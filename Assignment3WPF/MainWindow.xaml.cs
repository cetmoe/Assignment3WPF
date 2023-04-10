using Assignment3WPF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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

            // List of grades to pick from
            List<Grading> gradings = new List<Grading>
            {
                Grading.A,
                Grading.B,
                Grading.C,
                Grading.D,
                Grading.E,
                Grading.F
            };

            // Assign local storage
            Students = dx.Students.Local;
            Courses = dx.Courses.Local;
            Grades = dx.Grades.Local;


            // Load all data because the database is small
            dx.Students.Load();
            dx.Courses.Load();
            dx.Grades.Load();

            // Load students and order by student name.
            studentList.DataContext = Students.OrderBy(s => s.Studentname);
            // Load courses for combobox
            courseSelection.DataContext = Courses.ToObservableCollection();
            // Load grades for combobox
            gradeSelection.DataContext = gradings;

            // load all entries of grades equal to F
            failedCourses.DataContext = Grades
                .Where(g => Enum.Parse<Grading>(g.Grade1) == Grading.F);
        }
        private void studentSearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            studentList.DataContext = Students
                .Where(s => s.Studentname.ToLower().Contains(studentSearchText.Text.ToLower()))
                .OrderBy(s => s.Studentname);
        }

        private void courseSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Course selectedCourse = (Course)courseSelection.SelectedItem;
            studentsOfCourse.DataContext = Grades
                .Join(Courses,
                gr => gr.Coursecode,
                cr => cr.Coursecode,
                (go, co) => new { go.Student.Studentname, go.Grade1, go.Coursecode, co.Coursename })
                .Where(co => co.Coursecode.Contains(selectedCourse.Coursecode));
        }

        private void gradeSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Grading selectedGrade = (Grading)gradeSelection.SelectedItem;

            studentGrades.DataContext = Grades
                .Join(Courses,
                gr => gr.Coursecode,
                cr => cr.Coursecode,
                (go, co) => new { go.Student.Studentname, go.Grade1, go.Coursecode, co.Coursename })
                .Where(g => Enum.Parse<Grading>(g.Grade1) >= selectedGrade);
        }
    }
}
