# 📝 Online Exam System – Admin Panel & Exam Website

This project is a simple yet functional **Online Exam System** built using **ASP.NET Core** and **Entity Framework Core**. It consists of two main parts:

- **Admin Panel** – For managing exams and users.
- **User Website** – For registered users to take exams and view their scores.

---

## 🔐 Admin Panel

The Admin Panel allows administrators to:

- Log in securely using ASP.NET Identity
- Create, edit, and delete exams
- Add, update, and remove questions for each exam (each question includes a title, 4 choices, and one correct answer)
- Manually add users to the database (no self-registration)

---

## 👨‍🎓 User Website

The User Website allows users to:

- Log in with pre-added credentials
- View a list of available exams
- Take exams by answering multiple-choice questions
- Submit exams and immediately view:
  - Total score (as a percentage)
  - Number of correct and incorrect answers
  - Pass/fail status (60% is the passing threshold)

---

## 💡 Key Features

- Secure authentication using **ASP.NET Identity**
- Responsive UI with **Bootstrap**
- Interactive exam experience using **JavaScript** and **AJAX**
- Clean codebase using **Repository Pattern**
- Well-structured relational database with **Entity Framework Core Migrations**

---

## ⚙️ Technologies Used

- ASP.NET Core (MVC or Razor Pages)
- Entity Framework Core
- ASP.NET Identity
- SQL Server or In-Memory DB
- JavaScript (jQuery)
- Bootstrap
- AJAX

---

## 🧪 Evaluation Logic

Each question is worth 1 point.

**Score Calculation**:

```text
Score (%) = (Correct Answers / Total Questions) * 100
