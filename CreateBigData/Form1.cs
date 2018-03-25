using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
namespace CreateBigData
{
    
    public partial class Form1 : Form
    {
        SqlConnection _connection = null;
        String _connectString = "";
        private List<string> duong=new List<string>();
        private List<string> quan=new List<string>();
        private List<string> thanhpho=new List<string>();
        private List<string> HO = new List<string>();
        private List<string> TENLOT = new List<string>();
        private List<string> TEN = new List<string>();
       
        public string NonUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ","đ",
                                            "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ","í","ì","ỉ","ĩ","ị",
                                            "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                                            "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                                            "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a","d",
                                            "e","e","e","e","e","e","e","e","e","e","e",
                                            "i","i","i","i","i",
                                            "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                                            "u","u","u","u","u","u","u","u","u","u","u",
                                            "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }
        private void ReadFile()
        {
            StreamReader ho = new StreamReader("StoredFiles/Họ.txt");
            StreamReader tenlot = new StreamReader("StoredFiles/Tên lót.txt");
            StreamReader ten = new StreamReader("StoredFiles/Tên.txt");
            StreamReader street = new StreamReader("StoredFiles/Đường.txt");
            StreamReader district = new StreamReader("StoredFiles/Quận.txt");
            StreamReader city = new StreamReader("StoredFiles/Thành Phố.txt");
            string line = "";
            while ((line = ho.ReadLine()) != null)
                HO.Add(line.ToString());
            line = "";
            while ((line = tenlot.ReadLine()) != null)
                TENLOT.Add(line.ToString());
            line = "";
            while ((line = ten.ReadLine()) != null)
                TEN.Add(line);
            while ((line = city.ReadLine()) != null)
                thanhpho.Add(line.ToString());
            while ((line = district.ReadLine()) != null)
                quan.Add(line.ToString());
            while ((line = street.ReadLine()) != null)
                duong.Add(line.ToString());

        }


        public Form1()
        {
            InitializeComponent();
            _connectString = @"Data Source=DESKTOP-C4Q44NO\SQLEXPRESS;Initial Catalog=Hotel_Online;Integrated Security=True";
            _connection = new SqlConnection(_connectString);
            _connection.Open();
        }
        private string email(string Ho, string TenLot, string Ten, int id)
        {
            return NonUnicode(Ho[0].ToString() + TenLot[0].ToString() + Ten.ToString() + id.ToString()+"@gmail.com");
        }
        private string TenDangNhap(string Ho, string TenLot, string Ten,int id)
        {
            return NonUnicode(Ho[0].ToString() + TenLot[0].ToString() + Ten + id.ToString());
        }
        private string soCMND(int id)
        {
            string ID = id.ToString();
            Random ran = new Random();
            string zero = "0000000";
            string last = zero + ID;
            string result = (ran.Next(10000,99999).ToString()).Substring(0,3) + last.Substring(ID.Length,7);
            return result;
        }
        private string matKhau(string Ho, string TenLot, string Ten, int id)
        {
            return NonUnicode(Ho[0] + TenLot[0] + Ten + id.ToString());
        }
        private string dienThoai(int id)
        {
            string[] dauso = { "086", "096", "097", "098", "016", "016", "016", "091", "090","093" };
            string ID = id.ToString();
            Random ran = new Random();
            string duoi = ("000000000"+ID).Substring(ID.Length,9);
            string phone = dauso[ran.Next(0, 9)]+ duoi;
            return phone.Substring(0, 12);
        }
        private string MoTa()
        {
            string[] mt1 = { "Thích mua sắm", "Thích ăn uống", "Thích bơi lội", "Thích chụp hình","Thích động vật"};
            string[] mt2 = { "Sợ độ cao", "Sợ biển", "Không ăn được hải sản", "Sợ chó", "Sợ nắng" };
            string[] mt3 = { "Trầm tính", "Năng động", "Tự tin", "Thích khám phá", "Vui tính" };
            Random ran = new Random();
            int i = ran.Next(0, 4);
            return mt1[i] +", "+ mt2[i]+", " + mt3[i];
        }
        private string diaChi(int id)
        {
            Random ran = new Random();
            return id.ToString() + ", đường " + duong[ran.Next(0, 73)] + ", Phuong " + ran.Next(1, 17).ToString() + ", quận " + quan[ran.Next(0, 16)] + ", thành phố " + thanhpho[ran.Next(0, 72)];
        }
        private void AddMember(int n)
        {
            String procname = "sp_AddNewUser";
            SqlCommand _command = new SqlCommand(procname);
            _command = new SqlCommand(procname);
            _command.CommandType = CommandType.StoredProcedure;
            _command.Connection = _connection;
            _command.Parameters.Add("@hoTen", SqlDbType.NVarChar);
            _command.Parameters.Add("@tenDangNhap", SqlDbType.NVarChar);
            _command.Parameters.Add("@matKhau", SqlDbType.VarChar);
            _command.Parameters.Add("@soCMND", SqlDbType.Char);
            _command.Parameters.Add("@diaChi", SqlDbType.NVarChar);
            _command.Parameters.Add("@soDienThoai", SqlDbType.NVarChar);
            _command.Parameters.Add("@mota", SqlDbType.NVarChar);
            _command.Parameters.Add("@email", SqlDbType.NVarChar);
            int i = 0;
            string H = "";
            string TL = "";
            string T = "";
            string fullname = "";
            int count_Ho = HO.Count;
            int count_TenLot = TENLOT.Count;
            int count_Ten = TEN.Count;
            int index1 = 0,index2=0,index3 = 0;
            while (i < n && index1 < count_Ho)
            {
                H = HO[index1];
                index2 = 0;
                while (i < n && index2 < count_TenLot)
                {
                    TL = TENLOT[index2];
                    index3 = 0;
                    while (i < n && index3 < count_Ten)
                    {
                        T = TEN[index3];
                        fullname = H + " " + TL + " " + T;
                        _command.Parameters["@hoTen"].Value = fullname;
                        _command.Parameters["@tenDangNhap"].Value = TenDangNhap(H, TL, T, i);
                        _command.Parameters["@matKhau"].Value = matKhau(H, TL, T, i + 1);
                        _command.Parameters["@soCMND"].Value = soCMND(i + 1);
                        _command.Parameters["@diaChi"].Value = diaChi(i + 1);
                        _command.Parameters["@soDienThoai"].Value = dienThoai(i + 1);
                        _command.Parameters["@mota"].Value = MoTa();
                        _command.Parameters["@email"].Value = email(H, TL, T,i+1);
                        _command.ExecuteNonQuery();
                        i++;
                        index3++;
                    }
                    index2++;
                }
                index1++;
            }
            cbKhachHang.Checked = true;
        }

        private void AddHotel(int n)
        {
            String procname = "sp_AddHotel";
            SqlCommand _command = new SqlCommand(procname);
            _command.CommandType = CommandType.StoredProcedure;
            _command.Connection = _connection;
            _command.Parameters.Add("@tenKS", SqlDbType.NVarChar);
            _command.Parameters.Add("@soSao", SqlDbType.Int);
            _command.Parameters.Add("@soNha", SqlDbType.Char);
            _command.Parameters.Add("@duong", SqlDbType.NVarChar);
            _command.Parameters.Add("@quan", SqlDbType.NVarChar);
            _command.Parameters.Add("@thanhPho", SqlDbType.NVarChar);
            _command.Parameters.Add("@giaTB", SqlDbType.Int);
            _command.Parameters.Add("@moTa", SqlDbType.NVarChar);
            string Tenks = "";
            int sl = TEN.Count;
            int i = 0;
            Random ran = new Random();
            int[] gia = { 500, 1000, 1500, 2000, 4000 };
            string[] moTa = { "Phòng đẹp, rộng rãi",
                                " có bãi xe, có điều hòa",
                                " chấp nhận thú nuôi, wifi free",
                                " buffet sáng miễn phí",
                                " Spa,Gym miễn phí"};

            while (i < n && i < sl)
            {
                Tenks = TEN[i].ToString();
                int sosao = ran.Next(1, 5);
                string mt = "";
                for (int j = 0; j < sosao; j++)
                    mt = mt + ", " + moTa[j].ToString();
                _command.Parameters["@tenKS"].Value = Tenks.ToString();
                _command.Parameters["@soSao"].Value = sosao;
                _command.Parameters["@soNha"].Value = (i + 1).ToString();
                _command.Parameters["@duong"].Value = duong[ran.Next(0, 73)];
                _command.Parameters["@quan"].Value = quan[ran.Next(0, 16)];
                _command.Parameters["@thanhPho"].Value = thanhpho[ran.Next(0, 72)];
                _command.Parameters["@giaTB"].Value = gia[sosao - 1] * 1000;
                _command.Parameters["@moTa"].Value = mt;
                _command.ExecuteNonQuery();
                i++;
            }
            cbKhachSan.Checked = true;

        }
        private void AddTypeOfRoom(int n, int m)
        {
            string[] tenloaiphong = { "TWN", "DBL", "TRPL", "SGL","FML" };
            string[] motaloaiphong = { "2 giường đơn", "1 giường đôi", "3 giường đơn hoặc 1 giường đôi và 1 giường đơn", "1 giường đơn", "2 giường đôi" };
            string[] chuanphong = { "SUP", "DLX", "SUT", "STD", "SUTF"};
            string[] motachuan = { "Phòng rộng, view đẹp", "Rộng rãi, Thiết bị cao cấp", "Tầng cao, Sang trọng", "Diện tích nhỏ và view không đẹp nhưng giá rẻ","Tầng cao, view đẹp, thiết bị hiện đại, Dành cho gia đình" };

            //m là số dòng của khách sạn
            String procname = "sp_AddTypeOfRoom";

            SqlCommand _command = new SqlCommand(procname);
            _command.CommandType = CommandType.StoredProcedure;
            _command.Connection = _connection;
            _command.Parameters.Add("@tenloaiPhong", SqlDbType.NVarChar);
            _command.Parameters.Add("@maKS", SqlDbType.Int);
            _command.Parameters.Add("@donGia", SqlDbType.Int);
            _command.Parameters.Add("@moTa", SqlDbType.NVarChar);
            _command.Parameters.Add("@slTrong", SqlDbType.Int);

            int i = 0;
            Random ran = new Random();
            int[] giaphong = { 400, 500, 600, 200,800 };
            int[] giachuan = { 1500, 2500, 3500, 1000,4000 };
            int x = n / m;
            int y = 0;
            int index = 1;
            while (i < n)
            {
                for (int k = 0; k < 5 && i < n; k++)
                {
                    for (int g = 0; g < 5 && i < n; g++)
                    {
                        _command.Parameters["@tenloaiPhong"].Value = tenloaiphong[k] + " " + chuanphong[g];
                        _command.Parameters["@maKS"].Value = index;
                        _command.Parameters["@donGia"].Value = (giaphong[k] + giachuan[g]) * 1000;
                        _command.Parameters["@moTa"].Value = motaloaiphong[k] + ", " + motachuan[g];
                        _command.Parameters["@slTrong"].Value = ran.Next(5, 20);
                        _command.ExecuteNonQuery();
                        i++;
                        y++;
                        if (y == x)
                            break;
                    }
                    if (y == x)
                        break;
                }
                y = 0;
                index++;
                if (index > m)
                    index = m;
            }
            cbLoaiPhong.Checked = true;
        }
        private int slTrong(int id)
        {
            int sl = 0;
            String procname = "sp_return_slTrong";
            SqlCommand _command = new SqlCommand(procname);
            _command.CommandType = CommandType.StoredProcedure;
            _command.Connection = _connection;
            _command.Parameters.Add("@maLoaiPhong", SqlDbType.Int);
             _command.Parameters.Add("@rs", SqlDbType.Int);
            _command.Parameters["@maLoaiPhong"].Value = id;
            _command.Parameters["@rs"].Direction = ParameterDirection.Output;
            _command.ExecuteNonQuery();
            sl = (int)_command.Parameters["@rs"].Value;
            return sl;
        }
        private int SoPhong(int i)
        {
            return (i / 10 + 1) * 100 + i % 10;
        }
        private void AddRoomAndState(int n)
        {
            Random ran = new Random();
            String procname1 = "sp_AddRoom";
            String procname2 = "sp_AddStateOfRoom";
            SqlCommand _command1 = new SqlCommand(procname1);
            _command1.CommandType = CommandType.StoredProcedure;
            _command1.Connection = _connection;
            _command1.Parameters.Add("@loaiPhong", SqlDbType.Int );
            _command1.Parameters.Add("@soPhong", SqlDbType.Int);
            
            SqlCommand _command2 = new SqlCommand(procname2);
            _command2.CommandType = CommandType.StoredProcedure;
            _command2.Connection = _connection;
            _command2.Parameters.Add("@maPhong", SqlDbType.Int);
            _command2.Parameters.Add("@ngay", SqlDbType.DateTime);
            _command2.Parameters.Add("@tinhTrang", SqlDbType.NVarChar);
            int row = 1;
            n = n + 1;
            for(int i=1;i<n;i++)
            {
                //xử lý tình trạng phòng trống
                int Trong = slTrong(i);
                for(int j=0;j<Trong;j++)
                {
                    _command1.Parameters["@loaiPhong"].Value = i;
                    _command1.Parameters["@soPhong"].Value = SoPhong(j);
                    _command1.ExecuteNonQuery();
                    _command2.Parameters["@maPhong"].Value = row;
                    _command2.Parameters["@ngay"].Value = DateTime.Now;
                    _command2.Parameters["@tinhTrang"].Value = "còn trống";
                    _command2.ExecuteNonQuery();
                    row++;
                }
                int dbt=ran.Next(3, 7);
                for (int j = 0; j < Trong; j++)
                {
                    _command1.Parameters["@loaiPhong"].Value = i;
                    _command1.Parameters["@soPhong"].Value = SoPhong(j+Trong);
                    _command1.ExecuteNonQuery();
                    _command2.Parameters["@maPhong"].Value = row;
                    _command2.Parameters["@ngay"].Value = DateTime.Now;
                    _command2.Parameters["@tinhTrang"].Value = "đang bảo trì";
                    _command2.ExecuteNonQuery();
                    row++;
                }
            }
            cbPhong.Checked = true;
            cbTTPhong.Checked = true;
        }

        private void AddBookRoom(int n, int kh, int lp)
        {
            String procname = "sp_AddDatPhong";
            SqlCommand _command = new SqlCommand(procname);
            _command.CommandType = CommandType.StoredProcedure;
            _command.Connection = _connection;

            _command.Parameters.Add("@maLoaiPhong", SqlDbType.Int);
            _command.Parameters.Add("@maKH", SqlDbType.Int);
            _command.Parameters.Add("@ngayBD", SqlDbType.DateTime);
            _command.Parameters.Add("@ngayTra", SqlDbType.DateTime);
            _command.Parameters.Add("@mota", SqlDbType.NVarChar);
            _command.Parameters.Add("@tinhTrang", SqlDbType.NVarChar);

            string[] tinhtrang = { "Đã xác nhận", "Chưa xác nhận" };
            Random ran = new Random();
            DateTime start = DateTime.Now;
            start=start.AddDays(ran.Next(2,4));
            for (int i=0;i<n;i++)
            {
                _command.Parameters["@maLoaiPhong"].Value= ran.Next(1,(i+1)%lp);
                _command.Parameters["@maKH"].Value= ran.Next(1,(i+1)%kh);
                _command.Parameters["@ngayBD"].Value= start;
                _command.Parameters["@ngayTra"].Value= start.AddDays(ran.Next(3,15));
                _command.Parameters["@mota"].Value= "";
                _command.Parameters["@tinhTrang"].Value= tinhtrang[i%2];
                _command.ExecuteNonQuery();
            }
            cbDatPhong.Checked = true;

        }
        private void AddBill(int n)
        {
            n = n*2 + 1;

            String procname = "sp_AddBil";
            SqlCommand _command = new SqlCommand(procname);
            _command.CommandType = CommandType.StoredProcedure;
            _command.Connection = _connection;
            
            _command.Parameters.Add("@maDP", SqlDbType.Int);
            for (int i=1; i<n;i+=2)
            {
                _command.Parameters["@maDP"].Value = i;
                _command.ExecuteNonQuery();
            }
            cbHoaDon.Checked = true;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ReadFile();
            btnReset.Enabled = false;
        }
        private bool KtraDuLieu()
        {
            if (txtKhachHang.Text == "")
            {
                MessageBox.Show("Dữ liệu dòng của khách hàng không được trống");
                return false;
            }
            if (txtKhachSan.Text == "")
            {
                MessageBox.Show("Dữ liệu dòng của khách sạn không được trống");
                return false;
            }
            if (txtLoaiPhong.Text == "")
            {
                MessageBox.Show("Dữ liệu dòng của loại phòng không được trống");
                return false;
            }
            if (txtDatPhong.Text == "")
            {
                MessageBox.Show("Dữ liệu dòng của đặt phòng không được trống");
                return false;
            }
            if (txtHoaDon.Text == "")
            {
                MessageBox.Show("Dữ liệu dòng của hóa đơn không được trống");
                return false;
            }
            return true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (KtraDuLieu()==false)
                return;
            DateTime start = DateTime.Now;
            Number row = new Number();
            row.Khachhang = int.Parse(txtKhachHang.Text);
            row.Khachsan = int.Parse(txtKhachSan.Text);
            row.Loaiphong = int.Parse(txtLoaiPhong.Text);
            row.Datphong = int.Parse(txtDatPhong.Text);
            row.Hoadon = int.Parse(txtHoaDon.Text);

            AddMember(row.Khachhang);
            AddHotel(row.Khachsan);
            AddTypeOfRoom(row.Loaiphong, row.Khachsan);
            AddRoomAndState(row.Loaiphong);
            AddBookRoom(row.Datphong, row.Khachhang, row.Loaiphong);
            AddBill(row.Hoadon);
            MessageBox.Show("Đã khởi tạo thành công!" + "Thời gian khởi tạo: " + start.ToLongTimeString() + "- " + DateTime.Now.ToLongTimeString());
            btnTao.Enabled = false;
            btnReset.Enabled = true;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            String procname = "Reset_Database";
            SqlCommand _command = new SqlCommand(procname);
            _command.CommandType = CommandType.StoredProcedure;
            _command.Connection = _connection;
            _command.ExecuteNonQuery();
            btnTao.Enabled = true;
            btnReset.Enabled = false;
        }
    }
    public class Number
    {
        private int khachsan;
        private int khachhang;
        private int loaiphong;
        private int phong;
        private int trangthaiphong;
        private int datphong;
        private int hoadon;

        public int Khachsan { get => khachsan; set => khachsan = value; }
        public int Khachhang { get => khachhang; set => khachhang = value; }
        public int Loaiphong { get => loaiphong; set => loaiphong = value; }
        public int Phong { get => phong; set => phong = value; }
        public int Trangthaiphong { get => trangthaiphong; set => trangthaiphong = value; }
        public int Datphong { get => datphong; set => datphong = value; }
        public int Hoadon { get => hoadon; set => hoadon = value; }
    }
}
