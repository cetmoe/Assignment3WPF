using Assignment3WPF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assignment3WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly Dat154Context dx = new();

        private readonly LocalView<Student> Students;
        private readonly LocalView<Course> Courses;
        private readonly LocalView<Grade> Grades;

        public MainWindow()
        {
            InitializeComponent();

            List<Grading> gradings = new List<Grading>
            {
                Grading.A,
                Grading.B,
                Grading.C,
                Grading.D,
                Grading.E,
                Grading.F
            };

            Students = dx.Students.Local;
            Courses = dx.Courses.Local;
            Grades = dx.Grades.Local;

            dx.Students.Load();
            dx.Courses.Load();
            dx.Grades.Load();

            studentList.DataContext = Students.OrderBy(s => s.Studentname);
            courseSelection.DataContext = Courses.ToObservableCollection();
            gradeSelection.DataContext = gradings;
        }
        private void studentSearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            studentList.DataContext = Students
                .Where(s => s.Studentname.ToLower().Contains(studentSearchText.Text.ToLower()))
                .OrderBy(s => s.Studentname);
        }

        private void courseSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Course selectedCourse = (Course) courseSelection.SelectedItem;
            studentsOfCourse.DataContext = Grades
                .Join(Courses,
                gr => gr.Coursecode,
                cr => cr.Coursecode,
                (go, co) => new { go.Student.Studentname, go.Grade1, go.Coursecode })
                .Where(co => co.Coursecode.Contains(selectedCourse.Coursecode));

        }

        private void gradeSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Grading selectedGrade = (Grading) gradeSelection.SelectedItem;

            studentGrades.DataContext = Grades
                .Where(g => Enum.Parse<Grading>(g.Grade1) >= selectedGrade);
        }
    }
}
