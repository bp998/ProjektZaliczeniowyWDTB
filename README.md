# Projekt Zaliczeniowy - Programowanie aplikacji backendowych (.NET + Clean Architecture)

## Opis projektu

Aplikacja edukacyjna oparta o architekturę Clean Architecture, służąca do zarządzania studentami, kursami i zapisami na kursy. Projekt zawiera trzy warstwy prezentacji: REST API, GraphQL oraz Razor Pages z pełnym wsparciem dla JWT, ról systemowych i testów jednostkowych.

## Struktura rozwiązania

- **Domain** – encje, interfejsy repozytoriów
- **Application** – DTOs, logika biznesowa
- **Infrastructure** – implementacje repozytoriów, uwierzytelnianie, seeder danych
- **Presentation.WebApi** – REST API z JWT, Swaggerem i middleware
- **Presentation.GraphQL** – API GraphQL z wykorzystaniem HotChocolate
- **Presentation.AdminPanel** – Razor Pages (logowanie, panel admina, panel użytkownika, klient GraphQL)
- **Tests.Unit** – testy jednostkowe dla warstw
- `api-test.http`, `api-test.bat`, `api-test.sh` – skrypty testujące API (curl i REST Client)

## Technologie

- ASP.NET Core 8.0
- Entity Framework Core (InMemory)
- JWT Authentication
- Razor Pages
- GraphQL (HotChocolate)
- Swagger / Swashbuckle
- xUnit + Moq
- Clean Architecture

## Zakres funkcjonalny wg wymagań

### REST API
- Dostępne pod `http://localhost:5001/swagger`
- Minimum 3 encje: `Student`, `Course`, `Enrollment`
- Pełen CRUD dla każdej encji
- JWT z rolami (`Admin`, `User`)
- Middleware do logowania nagłówków HTTP
- Swagger UI dostępny pod `/swagger`
- Testy jednostkowe repozytoriów i kontrolerów
- Konfiguracja przez `appsettings.json`

### GraphQL
- Dostępne pod `http://localhost:5000/graphql`
- Implementacja z HotChocolate
- Query + Mutation (dla studentów, kursów, zapisów)
- Razor Pages jako klient (autoryzowany)
- Projekcja, sortowanie, filtrowanie

### Razor Pages
- Dostępne pod `http://localhost:5100`
- Logowanie z rolami
- Panel administratora i użytkownika
- Widoki dla REST i GraphQL
- Dynamiczne menu zależne od roli
- Obsługa tokenów i ciasteczek

### Testy API
- `api-test.http` (dla REST Client w Visual Studio)
- `api-test.bat` (skrypt do testowania przez curl z poziomu konsoli w systemach Windows)
- `api-test.sh` (skrypt do testowania przez curl w systemach Unix-like)

## Dane testowe

| Rola   | Login | Hasło     |
|--------|-------|-----------|
| Admin  | admin | admin123  |
| User   | user  | user123   |

## Jak uruchomić projekt

Aby w pełni przetestować wszystkie funkcjonalności aplikacji, **zaleca się uruchomienie wszystkich trzech projektów warstwy prezentacji jednocześnie**:

1. `Presentation.WebApi` – REST API i uwierzytelnianie (http://localhost:5001)

2. `Presentation.GraphQL` – serwer GraphQL (http://localhost:5000/graphql)

3. `Presentation.AdminPanel` – panel administracyjny i klient GraphQL (http://localhost:5100)

4. (Opcjonalnie) Uruchom `api-test.bat`, `api-test.http` lub `api-test.sh` w celu przetestowania API.

## Testowanie

### Uruchomienie testów jednostkowych:
``` bash
dotnet test Tests.Unit
```

Zawartość pokrywa testami m.in.:

1. Repozytoria (Student, Course, Enrollment)

2. AuthService (generowanie JWT)

3. Kontrolery REST API

### Przykładowe zapytania GraphQL:
```
query {
  students {
    id
    firstName
    lastName
    birthDate
  }
}

mutation {
  addStudent(firstName: "Anna", lastName: "Nowak", birthDate: "2000-01-01") {
    id
  }
}
```
