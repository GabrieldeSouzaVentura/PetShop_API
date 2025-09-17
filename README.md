# 🐱🐶 PetShop_API  
**Junior Backend Developer Task**

---

**Pet Shop** is a REST API for managing pet owners (guardians) and their pets.

## 🛠️ Built with C# using:

- **ASP.NET Core with Entity Framework Core**  
- **🔐 User authentication**  
- **📂 Relational database: SQLite**  
- **🏢Clean Architecture**

---

## 🔍 Key Features of Pet Shop:

- **User registration and login**  
- **Create, read, update, and delete Guardians (Tutores)**  
- **Organized guardian management**  
- **JWT authentication for `Users` and `Administrators`**  
- **Logging to monitor project behavior**

---

## 👥 User Roles

### 👨‍💻 Administrator

- **Create roles**  
- **Assign roles to users**  
- **Revoke roles**  
- **Add guardians and their pets**  
- **View guardians and their pets**  
- **Edit guardians and their pets**  
- **Delete guardians and their pets**

### 🧍‍♂️🧍‍♀️ User

- **View their own pets**

---

## 🛠️ Main Technologies, Frameworks, and Libraries:

- **Backend:** .NET C#  
- **Databases:** SQLite (in-memory)

---

## How to configure

Prerequisites
- **.Net 8.0**
- **Visual Studio 2022**

### 2. Clone repository
````
https://github.com/GabrieldeSouzaVentura/PetShop_API.git
````
- **open with visual studio 2022**

---

## 🔧Endpoints

### 🎮AuthController 
- `Login` - Login user
- `Register` - Register user (**Admin**)
- `RefreshToken` - Refresh Token
- `AddUserToRole` - Add users to roles (**Admin**)
- `GetUserInformations` - Get Informatins (**User**)
- `GetByNameUser{name}` - Get name user (**Admin**)
- `UpdateUser` - Update informations user (**Admin**)
- `Delete` - Delete user (**Admin**)

### 👩👨Tutor
- `RegisterTutor` - Register tutor (**Admin**)
- `GetByNameTutor` - Get name tutor (**Admin**)
- `GetAllInformationsTutors` - Get tutor informations (**User**)
- `UpdateTutor` - Update tutor informations (**Admin**)
- `DeleteTutor` - Delete tutor (**Admin**)


### 🐕🐈Pets
- `RegisterPet` - Register pets (**Admin**)
- `GetByNamePet` - Get name pets (**User**)
- `GetAllPets` - Get pets list (**User**)
- `UpdatePet` - Update pets informations (**Admin**)
- `DeletePet` - Delete pets (**Admin**)

---

## ⚠️Attention⚠️

- **🔧appsetings.json is configured in test mode, the secret key and other information must be stored in environment variables or using user secrets🔧**