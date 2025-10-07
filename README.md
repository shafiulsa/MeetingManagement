
#  Meeting Management System

An ASP.NET Core MVC-based Meeting Minutes Management System that allows users to manage and record meeting details, attendees, agendas, discussions, and decisions efficiently.

---

##  Features

### 🧾 Meeting Entry Form
- **Customer Selection**:
  - Choose between `Corporate` or `Individual` customer type using a radio button.
  - Dropdown dynamically populates customers from:
    - `Corporate_Customer_Tbl`
    - `Individual_Customer_Tbl`
- **Date & Time Selection**:
  - Bootstrap calendar and time picker (12-hour format, e.g., `03:30 PM`)
- **Meeting Information**:
  - Place, client-side attendees, host-side attendees
  - Agenda, discussion, and decision fields

###  Product/Service Section
- Product/Service dropdown loaded from `Products_Service_Tbl`
- Unit auto-filled in readonly textbox based on selected item
- Quantity input field (only numeric values allowed)
- Add multiple rows dynamically to a table (with edit/delete)
- Uses Bootstrap for dynamic interaction

###  Save Functionality
- First part saved into:
  - `Meeting_Minutes_Master_Tbl` via `Meeting_Minutes_Master_Save_SP`
- Second part saved into:
  - `Meeting_Minutes_Details_Tbl` via `Meeting_Minutes_Details_Save_SP`
- Saved using **stored procedures only**, not Entity Framework LINQ

### View Meeting Records
- Displays saved meeting minutes in a Bootstrap table
- Joined view of both master and detail tables
- Shows:
  - Customer type & name
  - Meeting datetime, attendees, agenda, discussion, and decision

---

##  Technologies Used

###  Backend
- **ASP.NET Core MVC (v8)**
- **C#**
- **Entity Framework Core (used only for executing SPs)**
- **MS SQL Server** with stored procedures

###  Frontend
- **HTML5, Bootstrap 5**
- **jQuery**
- **Font Awesome**

---

## Project Structure (Important Parts)

```

MeetingManagement/
├── Controllers/
│   ├── CustomerController.cs         --> Handles master/detail saving & customer loading
│   ├── HomeController.cs             --> Returns views like Index, MeetingMinutes
│   └── ProductController.cs          --> Loads product/services and handles product rows
├── Models/
│   ├── Corporate_Customer_Tbl.cs
│   ├── Individual_Customer_Tbl.cs
│   ├── Products_Service_Tbl.cs
│   ├── DisplayProduct.cs             --> For added product list in meeting
│   ├── Meeting_Minutes_Master_Tbl.cs
│   ├── Meeting_Minutes_Details_Tbl.cs
│   ├── MeetingMinutesFormData.cs
│   └── MeetingMinutesViewModel.cs
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml
│   │   ├── MeetingMinutes.cshtml     --> Main form + dynamic UI
│   │   └── GetData.cshtml            --> View saved data (MeetingMinutes list)
├── Data/
│   └── AppDbContext.cs
└── wwwroot/
├── css/meetingMinutes.css        --> Custom styling
└── js/meetingMinutes.js          --> Handles dynamic rows and AJAX

````

---

##  Entity Framework Commands

Make sure you have installed EF tools:
```bash
dotnet tool install --global dotnet-ef
````

###  Add Migration

```bash
dotnet ef migrations add InitialCreate
```

###  Update Database

```bash
dotnet ef database update
```

>  **Ubuntu Note**: Ensure you're in the folder where the `.csproj` file exists, and SQL Server is running.

---

##  Stored Procedures

### 1. `Meeting_Minutes_Master_Save_SP`

```sql
CREATE PROCEDURE [dbo].[Meeting_Minutes_Master_Save_SP]
    @CustomerType NVARCHAR(50),
    @CustomerId INT,
    @MeetingDateTime DATETIME,
    @MeetingPlace NVARCHAR(100),
    @ClientAttendees NVARCHAR(MAX),
    @HostAttendees NVARCHAR(MAX),
    @NewId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO Meeting_Minutes_Master_Tbls (
            CustomerType, CustomerId, MeetingDateTime, MeetingPlace, ClientAttendees, HostAttendees
        ) VALUES (
            @CustomerType, @CustomerId, @MeetingDateTime, @MeetingPlace, @ClientAttendees, @HostAttendees
        );
        SET @NewId = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
```

### 2. `Meeting_Minutes_Details_Save_SP`

```sql
CREATE PROCEDURE [dbo].[Meeting_Minutes_Details_Save_SP]
    @MasterId INT,
    @Agenda NVARCHAR(MAX),
    @Discussion NVARCHAR(MAX),
    @Decision NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO Meeting_Minutes_Details_Tbls (
            MasterId, Agenda, Discussion, Decision
        ) VALUES (
            @MasterId, @Agenda, @Discussion, @Decision
        );
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
```

---

##  How to Run

1. **Clone the repo**:

   ```bash
   git clone https://github.com/shafiulsa/MeetingManagement.git
   ```

2. **Open in** Visual Studio or VS Code

3. **Configure your DB** connection string in:

   ```
   appsettings.json
   ```

4. **Apply migration & run**:

   ```bash
   dotnet ef database update
   dotnet run
   ```

5. **Visit** the form:

   ```
   https://localhost:5001/Home/MeetingMinutes
   ```

---

##  UI Preview

###  Meeting Entry Form

<img width="1868" height="933" alt="image" src="https://github.com/user-attachments/assets/9864e32e-393b-4ab0-a250-274b18ec7441" />


###  Saved Records View

<img width="1848" height="922" alt="image" src="https://github.com/user-attachments/assets/394f24a8-edc4-42cc-b39d-051024bc8363" />

---

##  Author

**Shafiul Islam** <br>
🎓 Department of ICT, MBSTU <br>
🔗 GitHub: [@shafiulsa](https://github.com/shafiulsa)

---

## 📃 License

This project is for **educational and demonstration purposes only**. Not intended for production without additional security layers.

```


