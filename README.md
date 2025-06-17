# GoodMeal - AI Food Recommendation Web App

> **Kiến trúc 3-Layer với Blazor WebApp và PostgreSQL Database**

Dự án **GoodMeal** được xây dựng theo kiến trúc 3-layer, sử dụng Blazor WebApp cho giao diện người dùng và PostgreSQL database được host trên Docker.

---

## 🏗️ Kiến trúc 3-Layer

### 1. 🖥️ **Presentation Layer (Blazor WebApp)**
- **Thư mục**: `src/Blazor/`
- **Chức năng**: Giao diện người dùng, routing, và user interactions
- **Công nghệ**: Blazor Server-Side với Interactive Components
- **Pages**: 
  - `/` - Home page
  - `/users` - User management page

### 2. 💼 **Business Layer (Application + Domain)**
- **Thư mục**: `src/Application/` và `src/Domain/`
- **Chức năng**: 
  - **Domain**: Entities, business rules, repository interfaces
  - **Application**: Services, business logic, và data operations
- **Công nghệ**: .NET Core với Service pattern
- **Entities**: User, Role, UserRole, Job

### 3. 💾 **Data Access Layer (Infrastructure)**
- **Thư mục**: `src/Infrastructure/`
- **Chức năng**: Database access, repositories, migrations
- **Công nghệ**: Entity Framework Core với PostgreSQL
- **Components**: DbContext, Repository implementations, Migrations

---

# Application Environment
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=https://localhost:7001;http://localhost:5001
```

### Cách cấu hình:

1. **Sao chép file env.example**:
   ```bash
   cp env.example .env
   ```

2. **Cập nhật các giá trị trong .env** theo database của bạn

3. **Load environment variables**:
   
   **Windows (PowerShell)**:
   ```powershell
   .\scripts\load-env.ps1
   ```
   
   **Linux/macOS**:
   ```bash
   chmod +x scripts/load-env.sh
   source ./scripts/load-env.sh
   ```

---

## 🚀 Chạy Ứng Dụng

### 🎯 Quick Start (Recommended):

Chúng tôi đã tạo sẵn các script để chạy ứng dụng một cách dễ dàng:

**Linux/macOS**:
```bash
# Cấp quyền executable cho script (chỉ cần làm 1 lần)
chmod +x run-app.sh

# Chạy ứng dụng
./run-app.sh
```

**Windows (PowerShell)**:
```powershell
# Chạy ứng dụng
.\run-app.ps1
```

### 📋 Những gì script sẽ làm:

1. **🔧 Thiết lập Environment Variables**: Automatically configure database connection
2. **🏗️ Build Project**: Compile và prepare ứng dụng
3. **🚀 Start Server**: Khởi động Blazor server
4. **📊 Database Connection**: Kết nối tới PostgreSQL database trên Docker

### 📱 Truy cập ứng dụng:

Sau khi script chạy thành công, bạn có thể truy cập:

- **🏠 Home Page**: http://localhost:5086/
- **👥 Users Management**: http://localhost:5086/users
- **📝 Create/Edit Users**: Full CRUD operations

