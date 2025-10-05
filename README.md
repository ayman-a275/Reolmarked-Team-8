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
- **Stregkodegenerering:** ZXing.Net

## 📁 Projektstruktur

```
Reolmarked/
│
├── Reolmarked.sln                      
├── Reolmarked.csproj                   
├── appsettings.json                    
├── appsettings.Production.json         # Produktions konfiguration med connection string
├── App.xaml                            
├── App.xaml.cs                         
├── AssemblyInfo.cs                     
├── Styles.xaml                         # Globale UI styles
│
├── Model/                              # Domæne modeller
│   ├── Product.cs                      
│   ├── Shelf.cs                        
│   ├── ShelfType.cs                    
│   ├── Renter.cs                       
│   ├── RentalAgreement.cs              
│   ├── AgreementLine.cs                
│   ├── Transaction.cs                  
│   ├── TransactionLine.cs              
│   ├── PaymentMethod.cs                
│   ├── MonthlySettlement.cs            
│   ├── ShelfWithTypeName.cs            
│   ├── BookingShelf.cs                 
│   ├── SettlementSummary.cs           
│   └── RentalAgreementWithDetails.cs  
│
├── View/                               # XAML Views
│   ├── MainWindow.xaml                 
│   ├── MainWindow.xaml.cs             
│   ├── MenuView.xaml               
│   ├── MenuView.xaml.cs              
│   ├── MenuClosedView.xaml        
│   ├── MenuClosedView.xaml.cs       
│   ├── ProductView.xaml              
│   ├── ProductView.xaml.cs         
│   ├── ShelfView.xaml            
│   ├── ShelfView.xaml.cs       
│   ├── RenterView.xaml              
│   ├── RenterView.xaml.cs              
│   ├── RentShelfView.xaml         
│   ├── RentShelfView.xaml.cs     
│   ├── PaymentView.xaml       
│   ├── PaymentView.xaml.cs         
│   ├── MonthlySettlementView.xaml  
│   ├── MonthlySettlementView.xaml.cs  
│   ├── EditProductWindow.xaml    
│   ├── EditProductWindow.xaml.cs  
│   ├── EditShelfWindow.xaml      
│   └── EditShelfWindow.xaml.cs  
│
├── ViewModel/                          # ViewModels
│   ├── ProductViewModel.cs    
│   ├── ShelfViewModel.cs          
│   ├── RenterViewModel.cs          
│   ├── RentShelfViewModel.cs     
│   ├── PaymentViewModel.cs       
│   ├── MonthlySettlementViewModel.cs 
│   ├── EditProductViewModel.cs    
│   └── EditShelfViewModel.cs     
│
├── Command/                            # Command
│   └── RelayCommand.cs          
│
├── Helper/                             # Hjælpeklasser
│   ├── AppDbContext.cs                 # Entity Framework DbContext
│   ├── DbContextFactory.cs             # Factory til DbContext oprettelse
│   ├── SerialNumberGenerator.cs        # Genererer unikke 10 cifrede stregkoder
│   ├── PrintBarCode.cs                 # Printer stregkoder med ZXing
│   └── NegativeToTrueConverter.cs      # Value converters til UI binding
│
└── Data/                               # Ressourcer og filer
    └── img/                            # Billeder
        ├── close_menu.png              # Luk menu ikon
        ├── open_menu.png               # Åbn menu ikon
        └── shelfPlan.png               # Visuelt reolkort til booking
```

## 🚀 Installation


### Trin 1: Clone Repository
```bash
git clone https://github.com/ayman-a275/Reolmarked-Team-8.git
cd reolmarked
```

### Trin 2: Konfigurer Database
Projektet er konfigureret med en **online database** i appsettings.Production.json via smarterasp.net, som du kan bruge med det samme. Denne database er allerede sat op og klar til brug.
Hvis den online database er langsom eller ikke virker, kan du følge guiden nedenfor for at oprette en lokal database.

Opret Lokal Database **(Valgfrit)**
1. Opret en SQL Server database ved navn `ReolmarkedDatabase`
2. Opdater connection string i `appsettings.Production.json`:
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
CREATE TABLE PaymentMethod (
    PaymentMethodId INT PRIMARY KEY IDENTITY(1,1),
    PaymentMethodName NVARCHAR(100) NOT NULL
);

CREATE TABLE ShelfType (
    ShelfTypeId INT PRIMARY KEY IDENTITY(1,1),
    ShelfTypeName NVARCHAR(100) NOT NULL,
    ShelfTypeDescription NVARCHAR(500) NOT NULL,
    ShelfTypePrice DECIMAL(18,2) NOT NULL
);

CREATE TABLE Renter (
    RenterId INT PRIMARY KEY IDENTITY(1,1),
    RenterName NVARCHAR(200) NOT NULL,
    RenterTelephoneNumber NVARCHAR(20) NOT NULL,
    RenterEmail NVARCHAR(200) NOT NULL,
    RenterAccountNumber NVARCHAR(50) NOT NULL
);

CREATE TABLE Shelf (
    ShelfNumber INT PRIMARY KEY,
    ShelfRented BIT NOT NULL DEFAULT 0,
    ShelfTypeId INT NOT NULL,
    CONSTRAINT FK_Shelf_ShelfType FOREIGN KEY (ShelfTypeId) 
        REFERENCES ShelfType(ShelfTypeId)
);

CREATE TABLE RentalAgreement (
    RentalAgreementId INT PRIMARY KEY IDENTITY(1,1),
    RenterId INT NOT NULL,
    RentalAgreementDate DATETIME2 NOT NULL,
    RentalAgreementTotalPrice DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_RentalAgreement_Renter FOREIGN KEY (RenterId) 
        REFERENCES Renter(RenterId)
);

CREATE TABLE AgreementLine (
    AgreementLineId INT PRIMARY KEY IDENTITY(1,1),
    ShelfNumber INT NOT NULL,
    RentalAgreementId INT NOT NULL,
    CONSTRAINT FK_AgreementLine_Shelf FOREIGN KEY (ShelfNumber) 
        REFERENCES Shelf(ShelfNumber),
    CONSTRAINT FK_AgreementLine_RentalAgreement FOREIGN KEY (RentalAgreementId) 
        REFERENCES RentalAgreement(RentalAgreementId) ON DELETE CASCADE
);

CREATE TABLE Product (
    ProductSerialNumber NVARCHAR(50) PRIMARY KEY,
    ProductDescription NVARCHAR(500) NULL,
    ProductPrice DECIMAL(18,2) NOT NULL,
    ProductSold BIT NOT NULL DEFAULT 0,
    ShelfNumber INT NOT NULL,
    CONSTRAINT FK_Product_Shelf FOREIGN KEY (ShelfNumber) 
        REFERENCES Shelf(ShelfNumber)
);

CREATE TABLE [Transaction] (
    TransactionId INT PRIMARY KEY IDENTITY(1,1),
    TransactionDateTime DATETIME2 NOT NULL,
    TransactionTotalAmount DECIMAL(18,2) NOT NULL,
    TransactionPaidAmount DECIMAL(18,2) NOT NULL,
    PaymentMethodId INT NOT NULL,
    CONSTRAINT FK_Transaction_PaymentMethod FOREIGN KEY (PaymentMethodId) 
        REFERENCES PaymentMethod(PaymentMethodId)
);

CREATE TABLE TransactionLine (
    TransactionLineId INT PRIMARY KEY IDENTITY(1,1),
    TransactionId INT NOT NULL,
    ProductSerialNumber NVARCHAR(50) NOT NULL,
    CONSTRAINT FK_TransactionLine_Transaction FOREIGN KEY (TransactionId) 
        REFERENCES [Transaction](TransactionId) ON DELETE CASCADE,
    CONSTRAINT FK_TransactionLine_Product FOREIGN KEY (ProductSerialNumber) 
        REFERENCES Product(ProductSerialNumber)
);

CREATE TABLE MonthlySettlement (
    SettlementId INT PRIMARY KEY IDENTITY(1,1),
    RenterId INT NOT NULL,
    SettlementDate DATETIME2 NOT NULL,
    TotalRent DECIMAL(18,2) NOT NULL,
    TotalSales DECIMAL(18,2) NOT NULL,
    Commission DECIMAL(18,2) NOT NULL,
    NetAmount DECIMAL(18,2) NOT NULL,
    IsPaid BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_MonthlySettlement_Renter FOREIGN KEY (RenterId) 
        REFERENCES Renter(RenterId)
);

INSERT INTO PaymentMethod (PaymentMethodName) VALUES 
('Kontant'),
('MobilePay');

INSERT INTO ShelfType (ShelfTypeName, ShelfTypeDescription, ShelfTypePrice) VALUES 
('Standard Reol', 'Standard reol med 6 hylder', 850.00),
('Reol med Bøjlestang', 'Reol med 3 hylder og bøjlestang', 850.00);

INSERT INTO Shelf (ShelfNumber, ShelfRented, ShelfTypeId) VALUES 
(1, 0, 1), (2, 0, 1), (3, 0, 1), (4, 0, 1), (5, 0, 1),
(6, 0, 1), (7, 0, 1), (8, 0, 1), (9, 0, 1), (10, 0, 1),
(11, 0, 1), (12, 0, 1), (13, 0, 1), (14, 0, 2), (15, 0, 2),
(16, 0, 2), (17, 0, 2), (18, 0, 2), (19, 0, 1), (20, 0, 1),
(21, 0, 1), (22, 0, 1), (23, 0, 1), (24, 0, 1), (25, 0, 1),
(26, 0, 1), (27, 0, 1), (28, 0, 1), (29, 0, 1), (30, 0, 1),
(31, 0, 1), (32, 0, 1), (33, 0, 1), (34, 0, 1), (35, 0, 1),
(36, 0, 1), (37, 0, 1), (38, 0, 1), (39, 0, 1), (40, 0, 1),
(41, 0, 1), (42, 0, 1), (43, 0, 1), (44, 0, 1), (45, 0, 1),
(46, 0, 1), (47, 0, 1), (48, 0, 1), (49, 0, 1), (50, 0, 1),
(51, 0, 1), (52, 0, 1), (53, 0, 1), (54, 0, 1), (55, 0, 1),
(56, 0, 1), (57, 0, 1), (58, 0, 1), (59, 0, 1), (60, 0, 1),
(61, 0, 1), (62, 0, 1), (63, 0, 1), (64, 0, 1), (65, 0, 1),
(66, 0, 1), (67, 0, 1), (68, 0, 1), (69, 0, 1), (70, 0, 1),
(71, 0, 1), (72, 0, 1), (73, 0, 1), (74, 0, 1), (75, 0, 1),
(76, 0, 1), (77, 0, 1), (78, 0, 1), (79, 0, 1), (80, 0, 1);
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
2. Tilføj lejere via "Lejer" menuen
3. Udlej reoler via "Leje af reol" menuen
4. Registrer produkter via "Vare" menuen

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


## 📊 Database Model

```
ShelfType ── Shelf ──┬── Product ── TransactionLine ── Transaction ── PaymentMethod
                     │
                     └── AgreementLine ── RentalAgreement ──── Renter ── MonthlySettlement
                                                         
```

---

**Udviklet af Team 8**
