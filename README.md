# WORK IN PROGRESS
##  MindTrack Data Pipeline

A **high-performance C# data pipeline** for processing large-scale mental health datasets (600MB+ CSVs) with PostgreSQL integration. This system is a core component of the **MindTrack research platform**.

---

## Dataset Processing Flow

> Stream -> Parse -> Transform -> Analyze -> Bulk Insert -> Query

---

## Key Features

-  **Optimized CSV Parsing**
  - Uses `CsvHelper` for efficient, stream-based reading of large files (10,000+ records)
  
-  **PostgreSQL Bulk Import**
  - Efficient batched inserts using `Npgsql` and `Entity Framework Core`
  
-  **Data Analysis Toolkit**
  - Supports filtering, aggregation, and statistical insights

##  Dataset Source

 **[Social Anxiety Dataset on Kaggle](https://www.kaggle.com/datasets/natezhang123/social-anxiety-dataset#)**  
Provided by Nate Zhang – includes self-reported responses on mental health, lifestyle, and anxiety factors.

---

##  Technology Stack

### Core Framework
-  [.NET 8](https://dotnet.microsoft.com/)
-  Console application architecture with **dependency injection**

### Data Processing
-  [`CsvHelper`](https://joshclose.github.io/CsvHelper/) – high-performance CSV parsing
-  `Parallel LINQ (PLINQ)` – for parallel data operations (in progress)

### Database
-  PostgreSQL 16 – robust relational database
-  `Npgsql` – native PostgreSQL driver for .NET with bulk copy support

---

##  Example Use Case

```csharp
await dataReaderService.ImportLargeCsvAsync(); // Automatically parses and saves records in batches of 1000+
