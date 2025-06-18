#!/bin/bash

BASE_URL="http://localhost:5001/api"
LOGIN_ENDPOINT="$BASE_URL/auth/login"

echo "Logowanie jako admin..."
TOKEN=$(curl -s -X POST "$LOGIN_ENDPOINT" \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}' \
  | jq -r '.token')

if [[ "$TOKEN" == "null" || -z "$TOKEN" ]]; then
  echo "❌ Nie udało się zalogować. Sprawdź dane logowania."
  exit 1
fi

echo "Token JWT pobrany..."

# Dodawanie studenta
echo "Dodawanie studenta..."
STUDENT_ID=$(curl -s -X POST "$BASE_URL/student" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"firstName":"Test","lastName":"Student","birthDate":"2000-01-01"}' \
  | jq -r '.id')

# Dodawanie kursu
echo "Dodawanie kursu..."
COURSE_ID=$(curl -s -X POST "$BASE_URL/course" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"title":"GraphQL","description":"Wprowadzenie"}' \
  | jq -r '.id')

# Zapis studenta
echo "Zapisywanie studenta na kurs..."
curl -s -X POST "$BASE_URL/enrollment" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d "{\"studentId\":\"$STUDENT_ID\",\"courseId\":\"$COURSE_ID\",\"enrolledAt\":\"$(date -Iseconds)\"}"

# Pobierz studentów
echo "Lista studentów:"
curl -s -X GET "$BASE_URL/student" -H "Authorization: Bearer $TOKEN" | jq

# 🗑️ Usuń zapisy i dane
echo "Usuwanie studenta..."
curl -s -X DELETE "$BASE_URL/student/$STUDENT_ID" -H "Authorization: Bearer $TOKEN"
echo "Usuwanie kursu..."
curl -s -X DELETE "$BASE_URL/course/$COURSE_ID" -H "Authorization: Bearer $TOKEN"

echo "Test zakończony pomyślnie."
