# Reolmarked projekt - WPF Applikation

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)
![WPF](https://img.shields.io/badge/WPF-Windows-0078D4?logo=windows)
![C#](https://img.shields.io/badge/C%23-239120?logo=csharp)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoftsqlserver)

## 📋 Om Projektet

Dette er en WPF desktop applikation udviklet af **Team 8** til et Reolmarked projekt i Datamatiker uddannelsen.

Systemet håndterer udlejning af 80 reoler, produktregistrering med stregkoder, betalingshåndtering og månedlig afregning til lejere.

### 🎯 Projektperiode
**02-09-2025 til 06-10-2025**

## ✨ Funktioner

- **Reol Administration**
  - Opret og rediger reoler med priser
  - Visuel oversigt over ledige og udlejede reoler
  - Fleksible lejepriser baseret på antal reoler

- **Lejer Håndtering**
  - Registrering af lejere med kontaktinformation
  - Udlejning af reoler med visuel planoversigt
  - Månedlig afregning af salg og leje

- **Produkt Registrering**
  - Generering af stregkoder
  - Tilknytning af produkter til specifikke reoler
  - Prissætning og lagerstyring

- **Betaling & Salg**
  - Registrering af salg ved stregkode scanning
  - Multiple betalingsmetoder
  - Automatisk beregning af byttepenge

- **Afregning**
  - Månedlig opgørelse pr. lejer
  - 10% kommission på alt salg
  - Automatisk modregning af leje

## 🛠️ Teknologier

- **Framework:** .NET 9.0
- **UI:** WPF (Windows Presentation Foundation)
- **Arkitektur:** MVVM (Model-View-ViewModel)
- **Database:** Microsoft SQL Server

## 📁 Projektstruktur

```
Reolmarked/
├── Model/                  # Domæne modeller
│   ├── Product.cs
│   ├── Rack.cs
│   ├── Renter.cs
│   ├── RentedRack.cs
│   ├── Transaction.cs
│   ├── TransactionLine.cs
│   ├── PaymentMethod.cs
│   └── BookingRack.cs
├── View/                   # XAML views
│   ├── MainWindow.xaml
│   ├── ProductView.xaml
│   ├── RackView.xaml
│   ├── RenterView.xaml
│   ├── RentRackView.xaml
│   ├── PaymentView.xaml
│   ├── MenuView.xaml
│   ├── MenuClosedView.xaml
│   └── EditRackWindow.xaml
├── ViewModel/              # View models
│   ├── ProductViewModel.cs
│   ├── RackViewModel.cs
│   ├── RenterViewModel.cs
│   ├── RentRackViewModel.cs
│   ├── PaymentViewModel.cs
│   └── EditRackViewModel.cs
├── Data/                   # Database context
│   └── AppDbContext.cs
├── Command/                # Command pattern
│   └── RelayCommand.cs
├── Helper/                 # Hjælpeklasser
│   └── NumberGenerator.cs
└── appsettings.json        # Konfiguration
```

## 🚀 Installation


### Trin 1: Clone Repository
```bash
git clone https://github.com/ayman-a275/Reolmarked-Team-8.git
cd reolmarked
```

### Trin 2: Konfigurer Database
1. Opret en SQL Server database ved navn `ReolmarkedDatabase`
2. Opdater connection string i `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=DIN_SERVER;Database=ReolmarkedDatabase;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### Trin 3: Opret Database Tabeller
Kør følgende SQL script for at oprette tabellerne:

```sql
CREATE TABLE Rack (
    RackNumber INT PRIMARY KEY,
    RackPrice DECIMAL(18,2) NOT NULL,
    RackRented BIT NOT NULL DEFAULT 0
);

CREATE TABLE Renter (
    RenterId INT PRIMARY KEY IDENTITY(1,1),
    RenterName NVARCHAR(100) NOT NULL,
    RenterTelephoneNumber NVARCHAR(20) NOT NULL,
    RenterEmail NVARCHAR(100)
);

CREATE TABLE RentedRack (
    RentedRackId INT PRIMARY KEY IDENTITY(1,1),
    RackNumber INT NOT NULL,
    RenterId INT NOT NULL,
    RentedRackAgreedPrice DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (RackNumber) REFERENCES Rack(RackNumber),
    FOREIGN KEY (RenterId) REFERENCES Renter(RenterId)
);

CREATE TABLE Product (
    ProductSerialNumber NVARCHAR(50) PRIMARY KEY,
    ProductPrice DECIMAL(18,2) NOT NULL,
    ProductSold BIT NOT NULL DEFAULT 0,
    RackNumber INT NOT NULL,
    FOREIGN KEY (RackNumber) REFERENCES Rack(RackNumber)
);

CREATE TABLE PaymentMethod (
    PaymentMethodId INT PRIMARY KEY IDENTITY(1,1),
    PaymentMethodName NVARCHAR(50) NOT NULL
);

CREATE TABLE [Transaction] (
    TransactionId INT PRIMARY KEY IDENTITY(1,1),
    TransactionDateTime DATETIME NOT NULL,
    TransactionTotalAmount DECIMAL(18,2) NOT NULL,
    TransactionPaidAmount DECIMAL(18,2) NOT NULL,
    PaymentMethodId INT NOT NULL,
    FOREIGN KEY (PaymentMethodId) REFERENCES PaymentMethod(PaymentMethodId)
);

CREATE TABLE TransactionLine (
    TransactionLineId INT PRIMARY KEY IDENTITY(1,1),
    TransactionId INT NOT NULL,
    ProductSerialNumber NVARCHAR(50) NOT NULL,
    FOREIGN KEY (TransactionId) REFERENCES [Transaction](TransactionId),
    FOREIGN KEY (ProductSerialNumber) REFERENCES Product(ProductSerialNumber)
);

INSERT INTO PaymentMethod (PaymentMethodName) VALUES 
('Kontant'),
('MobilePay'),
('Dankort');
```

### Trin 4: Build og Kør
```bash
dotnet restore
dotnet build
dotnet run --project Reolmarked/Reolmarked.csproj
```

## 💡 Brug

### Første Gang
1. Start applikationen
2. Opret reoler via "Reol" menuen
3. Tilføj lejere via "Lejer" menuen
4. Udlej reoler via "Leje af reol" menuen
5. Registrer produkter via "Vare" menuen

### Daglig Brug
1. Scan eller indtast produktets stregkode ved salg
2. Vælg betalingsmetode
3. Registrer betalingen

### Månedlig Afregning
Systemet beregner automatisk:
- Total salg pr. reol
- 10% kommission
- Leje for næste måned
- Udbetaling til lejer

## 🎨 UI Design

Applikationen bruger et moderne, farvekodet design:
- **Grøn (#00a67d):** Ledige reoler
- **Rød (#cb2d6f):** Udlejede reoler
- **Blå (#0600d1):** Valgte reoler
- **Bootstrap-inspireret** farveskema for knapper og menuer

## 📊 Database Model

```
Rack ──┬── RentedRack ── Renter
       │
       └── Product ── TransactionLine ── Transaction ── PaymentMethod
```

---

**Udviklet af Team 8**
