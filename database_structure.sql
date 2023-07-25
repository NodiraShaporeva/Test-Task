-- Структура базы данных:

-- Таблица "Courses":
-- CourseID (PRIMARY KEY) - уникальный идентификатор курса.
-- CourseName - название курса.
-- MaxCapacity - максимальное количество мест на курсе.

-- Таблица "Students":
-- StudentID (PRIMARY KEY) - уникальный идентификатор обучаемого.
-- StudentName - имя обучаемого.
-- StudentEmail - электронная почта обучаемого (если необходимо).

-- Таблица "Registrations" (Записи на курсы):
-- CourseID (FOREIGN KEY) - ссылка на CourseID из таблицы Courses.
-- StudentID (FOREIGN KEY) - ссылка на StudentID из таблицы Students.
-- RegistrationDate - дата записи обучаемого на курс.

-- Здесь используется составной ключ (CourseID, StudentID) для таблицы "Registrations",
-- чтобы обеспечить уникальность комбинации курса и обучаемого, что соответствует требованию #3.

-- Создание таблицы "Courses"
CREATE TABLE Courses (
    CourseID INT PRIMARY KEY,
    CourseName VARCHAR(100),
    MaxCapacity INT
);

-- Создание таблицы "Students"
CREATE TABLE Students (
    StudentID INT PRIMARY KEY,
    StudentName VARCHAR(100),
    StudentEmail VARCHAR(100)
);

-- Создание таблицы "Registrations"
CREATE TABLE Registrations (
    CourseID INT,
    StudentID INT,
    RegistrationDate DATE,
    PRIMARY KEY (CourseID, StudentID),
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID),
    FOREIGN KEY (StudentID) REFERENCES Students(StudentID)
);