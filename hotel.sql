create database Hotel_Online
use Hotel_Online

create table KhachHang
(
	maKH int identity(1,1),
	hoTen nvarchar(50) not null,
	tenDangNhap nvarchar(50) not null unique,
	matKhau nvarchar(50) not null,
	soCMND nvarchar(10) not null unique,
	diaChi nvarchar(100),
	soDienThoai char(12) unique,
	moTa nvarchar(100),
	email nvarchar(50) unique,
	primary key(maKH)
)
create table KhachSan
(
	maKS int identity(1,1),
	tenKS nvarchar(30) not null,
	soSao int not null,
	soNha char(10),
	duong nvarchar(30),
	quan nvarchar(30),
	thanhPho nvarchar(30),
	giaTB int,
	moTa nvarchar(100),
	primary key(maKS)
)

create table LoaiPhong
(
	maLoaiPhong int identity(1,1),
	tenLoaiPhong nvarchar(30) not null,
	maKS int not null,
	donGia int not null,
	moTa nvarchar(100),
	slTrong int not null,
	primary key(maLoaiPhong),
)
create table Phong
(
	maPhong int identity(1,1),
	loaiPhong int not null,
	soPhong int not null,
	primary key(maPhong)
)
create table TrangThaiPhong
(
	maPhong int,
	ngay datetime,
	tinhTrang nvarchar(15),
	primary key(maPhong,ngay)
)
create table DatPhong
(
	maDP int identity(1,1),
	maLoaiPhong int not null,
	maKH int not null,
	ngayBatDau datetime,
	ngayTraPhong datetime,
	ngayDat datetime,
	donGia int not null,
	moTa nvarchar(100),
	tinhTrang nvarchar(15),
	primary key(maDP)
)
create table HoaDon
(
	maHD int identity(1,1),
	ngayThanhToan datetime not null,
	tongTien int not null,
	maDP int not null,
	primary key(maHD)
)

alter table HOADON
add constraint FK_HOADON_DATPHONG
foreign key(maDP)
references DatPhong(maDP)

alter table DatPhong
add constraint FK_DATPHONG_KHACHHANG
foreign key (maKH)
references KhachHang(maKH)

alter table DatPhong
add constraint FK_DATPHONG_PHONG
foreign key (maLoaiPhong)
references LoaiPhong(maLoaiPhong)

alter table Phong
add constraint FK_PHONG_LOAIPHONG
foreign key (loaiPhong)
references LoaiPhong(maLoaiPhong)

alter table LoaiPhong
add constraint FK_LOAIPHONG_KHACHSAN
foreign key (maKS)
references KhachSan(maKS)

alter table TrangThaiPhong
add constraint FK_TRANGTHAIPHONG_PHONG
foreign key (maPhong)
references Phong(maPhong)

create procedure sp_AddNewUser(
	@hoTen nvarchar(30),
	@tenDangNhap nvarchar(30),
	@matKhau varchar(30),
	@soCMND char(15),
	@diaChi nvarchar(100),
	@soDienThoai char(12),
	@moTa nvarchar(100),
	@email nvarchar(30))
as
	insert into KhachHang values(@hoTen,@tenDangNhap,@matKhau,@soCMND,@diaChi,@soDienThoai,@moTa,@email)

create procedure sp_AddHotel(
	@tenKS nvarchar(30),
	@soSao int,
	@soNha char(10),
	@duong nvarchar(30),
	@quan nvarchar(30),
	@thanhPho nvarchar(30),
	@giaTB int,
	@moTa nvarchar(100))
as
	insert into KhachSan values(@tenKS,@soSao,@soNha,@duong,@quan,@thanhPho,@giaTB,@moTa)
	
create procedure sp_AddTypeOfRoom(
	@tenLoaiPhong nvarchar(30),
	@maKS int,
	@donGia int,
	@moTa nvarchar(100),
	@slTrong int)
as
	insert into LoaiPhong values(@tenLoaiPhong,@maKS,@donGia,@moTa,@slTrong)

create procedure sp_AddRoom(
	@loaiPhong int,
	@soPhong int)
as 
	insert into Phong values(@loaiPhong,@soPhong)
exec sp_AddRoom 1,100
create procedure sp_AddStateOfRoom(
	@maPhong int,
	@ngay datetime,
	@tinhTrang nvarchar(15))
as
	insert into TrangThaiPhong values(@maPhong,@ngay,@tinhTrang)

create procedure sp_return_slTrong(
	@maLoaiPhong int,
	@rs int =0 output)
as
begin
	select @rs=slTrong
	from LoaiPhong
	where @maLoaiPhong=maLoaiPhong
	return @rs
end

create procedure sp_return_gia(
	@maLoaiPhong int,
	@donGia int output)
as
begin
	
end

create procedure sp_AddDatPhong(
	@maLoaiPhong int,
	@maKH int,
	@ngayBD datetime,
	@ngayTra datetime,
	@mota nvarchar(1000),
	@tinhTrang nvarchar(15))
as
begin
	declare @ngayDat datetime
	set @ngayDat= getdate()

	declare @donGia int =0
	select @donGia=LP.donGia
	from LoaiPhong LP
	where @maLoaiPhong=LP.maLoaiPhong

	declare @maPhong int =0;
	select top 1 @maPhong=P.maPhong
	from Phong P, TrangThaiPhong TTP
	where P.loaiPhong=@maLoaiPhong and TTP.maPhong=P.maPhong and TTP.tinhTrang=N'còn trống'

	update TrangThaiPhong
	set tinhTrang=N'đang sử dụng'
	where maPhong=@maPhong

	update LoaiPhong
	set slTrong = slTrong -1
	where maLoaiPhong=@maLoaiPhong

	insert into DatPhong values(@maLoaiPhong,@maKH,@ngayBD,@ngayTra,@ngayDat,@donGia,@mota,@tinhTrang)
end

create procedure sp_AddBil(
	
	@maDP int)
as
begin
	declare @ngayThanhToan datetime
	declare @tongTien int
	declare @ngaybd datetime
	select @ngaybd=DP.ngayBatDau
	from DatPhong DP
	where DP.maDP=@maDP 

	select @ngayThanhToan=DP.ngayTraPhong
	from DatPhong DP
	where DP.maDP=@maDP

	declare @maLoaiPhong int
	select @maLoaiPhong=DP.maLoaiPhong
	from DatPhong DP
	where DP.maDP=@maDP

	declare @donGia int =0
	select @donGia=LP.donGia
	from LoaiPhong LP
	where @maLoaiPhong=LP.maLoaiPhong

	declare @maPhong int =0;
	select top 1 @maPhong=P.maPhong
	from Phong P, TrangThaiPhong TTP
	where P.loaiPhong=@maLoaiPhong and TTP.maPhong=P.maPhong and TTP.tinhTrang=N'đang sử dụng'

	update TrangThaiPhong
	set tinhTrang=N'còn trống'
	where maPhong=@maPhong

	update LoaiPhong
	set slTrong = slTrong + 1
	where maLoaiPhong=@maLoaiPhong

	declare @ngayo int
	set @ngayo=datediff(day,@ngaybd,@ngayThanhToan)
	set @tongtien=@ngayo*@donGia
	if(exists( select* from DatPhong DP where DP.maDP=@maDP and DP.tinhTrang=N'Đã xác nhận'))
	begin
		insert into HoaDon values(@ngayThanhToan, @tongTien,@maDP)
	end

end
select*from KhachHang
create procedure Reset_Database
as
begin
	delete HoaDon
	DBCC checkident(HoaDon,reseed,0)
	
	delete DatPhong
	DBCC checkident(DatPhong,reseed,0)

	delete TrangThaiPhong
	delete Phong
	DBCC checkident(Phong,reseed,0)

	delete LoaiPhong
	DBCC checkident(LoaiPhong,reseed,0)

	delete KhachSan
	DBCC checkident(KhachSan,reseed,0)

	delete KhachHang
	DBCC checkident(KhachHang,reseed,0)
end
exec Reset_Database