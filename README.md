## Security Incident Tracker - ghid de instalare și rulare

Acest proiect este compus din două părți care funcționează împreună:
1. Serverul Web (Backend & Site Admin) - realizat în ASP.NET Core Razor Pages.
2. Aplicația Mobilă (Client) - realizată în .NET MAUI/Xamarin pentru Android.

Pentru ca aplicația să funcționeze corect, Serverul trebuie să fie pornit înaintea Aplicației Mobile!!!

## Cerințe preliminare

Visual Studio 2022 cu următoarele workload-uri instalate:
- ASP.NET and web development;
- .NET Multi-platform App UI development (MAUI) sau Mobile development with .NET;
- SQL Server Express sau LocalDB (instalat implicit cu Visual Studio).

Pasul 1: Configurarea bazei de date
Înainte de prima rulare, baza de date trebuie creată.

1. Deschideți soluția (.sln) în Visual Studio.
2. Deschideți Package Manager Console (din meniul Tools -> NuGet Package Manager).
3. Asigurați-vă că la "default project" este selectat proiectul Web (SecurityIncidentTrackerWebApp).
4. Rulați comanda: Update-Database (această comandă va crea baza de date locală și tabelele necesare (Users, Incidents, etc)).

Pasul 2: Pornirea aplicației (ordinea corectă)
Există două metode de a porni proiectul. Recomand metoda A pentru simplitate.

Metoda A: Multiple Startup Projects (această metodă configurează Visual Studio să deschidă ambele proiecte simultan).
1. În Solution Explorer, faceți click dreapta pe Solution 'SecurityIncidentTracker' (prima linie de sus).
2. Selectați Set Startup Projects...
3. Bifați opțiunea multiple startup projects.
4. Configurați acțiunile astfel:
    - SecurityIncidentTrackerWebApp (Server): Setat pe Start.
    - SecurityIncidentTrackerMobileApp (Mobile): Setat pe Start.
5. Apăsați Apply și OK.
6. Apăsați butonul Start (simbolul verde Play) din bara de sus a Visual Studio.

Metoda B: Pornire manuală (pas cu pas)
Dacă doriți să controlați procesul manual:
1. Faceți click dreapta pe proiectul Web -> Debug -> Start New Instance.
2. Se va deschide browserul cu site-ul de administrare. Lăsați-l deschis!
3. Reveniți în Visual Studio.
4. Faceți click dreapta pe proiectul Mobile -> Debug -> Start New Instance.
5. Se va deschide emulatorul Android.

## Note importante pentru emulator

Conexiunea la Server: 
Aplicația mobilă este configurată să comunice cu serverul local. Dacă folosiți Emulatorul Android standard, adresa serverului este configurată ca 10.0.2.2 (care este echivalentul localhost pentru emulator).

SSL/HTTPS: 
În mediul de dezvoltare, certificatul SSL poate cauza erori pe emulator. Din acest motiv, redirecționarea HTTPS poate fi dezactivată temporar în Program.cs pentru testare.

## Credențiale de test

Dacă baza de date a fost populată (seed), puteți folosi următorul cont pentru site-ul de administrare:
Email: admin@test.com 
Parolă: Parola123! 

## Dacă apar erori

Eroare "Connection refused": 
Verificați dacă proiectul Web (Serverul) rulează și dacă browserul este deschis.

Eroare bază de date: 
Asigurați-vă că ați rulat Update-Database în consolă.

## Tehnologii folosite:
* Framework: .NET 8.0
* Database: SQL Server (via Entity Framework Core)
* Backend: ASP.NET Core Razor Pages + Web API
* Mobile: .NET MAUI
* Pachete Cheie:
    - Newtonsoft.Json (serializare date);
    - Microsoft.AspNetCore.Identity (sistem de login).