using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BaiTapB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Model1 context = new Model1();
                List<Faculty> listFaculties = context.Faculties.ToList();
                List<Student> listStudents = context.Students.ToList();
                FillFacultyCombobox(listFaculties);
                BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillFacultyCombobox(List<Faculty> listFaculties)
        {
            cmbKhoa.DataSource = listFaculties;
            cmbKhoa.DisplayMember = "FacultyName";
            cmbKhoa.ValueMember = "FacultyID";
        }

        private void BindGrid(List<Student> listStudents)
        {
            dataGridView.Rows.Clear();
            foreach (Student student in listStudents)
            {
                int index = dataGridView.Rows.Add();
                dataGridView.Rows[index].Cells[0].Value = student.StudentID;
                dataGridView.Rows[index].Cells[1].Value = student.StudentName;
                dataGridView.Rows[index].Cells[2].Value = student.Faculty.FacultyName;
                dataGridView.Rows[index].Cells[3].Value = student.AverageScore;
            }
        }

        private void LoadData()
        {
            using (Model1 context = new Model1())
            {
                
                List<Student> listStudents = context.Students.ToList();
                BindGrid(listStudents);
            }
        }
        private void ResetForm()
        {
            txtMSSV.Clear();
            txtTenSV.Clear();
            txtDiemTB.Clear();
            cmbKhoa.SelectedIndex = 0; 
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMSSV.Text) || string.IsNullOrWhiteSpace(txtTenSV.Text) || string.IsNullOrWhiteSpace(txtDiemTB.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                if (txtMSSV.Text.Trim().Length != 10)
                {
                    MessageBox.Show("Mã số sinh viên phải có 10 kí tự!");
                    return;
                }

                if (!float.TryParse(txtDiemTB.Text.Trim(), out float avgScore) || avgScore < 0 || avgScore > 10)
                {
                    MessageBox.Show("Điểm trung bình phải nằm trong khoảng từ 0 đến 10!");
                    return;
                }

                Student newStudent = new Student
                {
                    StudentID = txtMSSV.Text.Trim(),
                    StudentName = txtTenSV.Text.Trim(),
                    AverageScore = avgScore,
                    FacultyID = cmbKhoa.SelectedValue.ToString()
                };

                using (Model1 context = new Model1())
                {
                    context.Students.Add(newStudent);
                    context.SaveChanges();
                }

                LoadData();
                MessageBox.Show("Thêm mới dữ liệu thành công!");

                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {


            try
            {
                if (string.IsNullOrWhiteSpace(txtMSSV.Text) || string.IsNullOrWhiteSpace(txtTenSV.Text) || string.IsNullOrWhiteSpace(txtDiemTB.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                if (txtMSSV.Text.Trim().Length != 10)
                {
                    MessageBox.Show("Mã số sinh viên phải có 10 kí tự!");
                    return;
                }

                if (!float.TryParse(txtDiemTB.Text.Trim(), out float avgScore) || avgScore < 0 || avgScore > 10)
                {
                    MessageBox.Show("Điểm trung bình phải nằm trong khoảng từ 0 đến 10!");
                    return;
                }

                using (Model1 context = new Model1())
                {
                    string studentID = txtMSSV.Text.Trim();
                    Student existingStudent = context.Students.FirstOrDefault(s => s.StudentID == studentID);

                    if (existingStudent != null)
                    {
                        existingStudent.StudentName = txtTenSV.Text.Trim();
                        existingStudent.AverageScore = avgScore;
                        existingStudent.FacultyID = cmbKhoa.SelectedValue.ToString();

                        context.SaveChanges();
                        MessageBox.Show("Cập nhật dữ liệu thành công!");

                        LoadData();

                        ResetForm();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy MSSV cần sửa!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                using (Model1 context = new Model1())
                {
                    string studentID = txtMSSV.Text.Trim();
                    Student studentToDelete = context.Students.FirstOrDefault(s => s.StudentID == studentID);

                    if (studentToDelete != null)
                    {
                        context.Students.Remove(studentToDelete);
                        context.SaveChanges();
                        MessageBox.Show("Xóa sinh viên thành công!");

                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn thoát", "Đồng ý thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
