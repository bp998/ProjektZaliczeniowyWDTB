@echo off
echo === Logging in as admin ===
curl -X POST http://localhost:5001/api/auth/login ^
     -H "Content-Type: application/json" ^
     -d "{\"username\":\"admin\", \"password\":\"admin123\"}" ^
     -c cookies.txt
echo.

echo === Getting all students ===
curl -X GET http://localhost:5001/api/student ^
     -b cookies.txt
echo.

echo === Adding a new student ===
curl -X POST http://localhost:5001/api/student ^
     -H "Content-Type: application/json" ^
     -b cookies.txt ^
     -d "{\"firstName\":\"Jan\",\"lastName\":\"Kowalski\",\"birthDate\":\"2000-01-01\"}"
echo.

echo === Getting all courses ===
curl -X GET http://localhost:5001/api/course ^
     -b cookies.txt
echo.

echo === Enrolling student to course ===
curl -X POST http://localhost:5001/api/enrollment ^
     -H "Content-Type: application/json" ^
     -b cookies.txt ^
     -d "{\"studentId\":\"PUT_STUDENT_ID_HERE\",\"courseId\":\"PUT_COURSE_ID_HERE\"}"
echo.

echo === Done ===
pause
