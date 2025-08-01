
# 🌐 DotnetFilePortal Web

**DotnetFilePortal Web** es una aplicación frontend construida en **ASP.NET Core MVC (.NET 8)** que actúa como la interfaz de usuario para la plataforma de gestión de archivos. Este frontend se comunica exclusivamente con el **DotnetFilePortal Gateway**, el cual enruta las peticiones hacia la API protegida.

---

## 🧩 Características principales

### 🔐 Autenticación y Registro
- Permite a los usuarios registrarse y autenticarse mediante formularios MVC.
- Las credenciales se envían como `POST` al **gateway**, que redirige a la API de autenticación.
- Al iniciar sesión correctamente, el backend emite un **JSON Web Token (JWT)**.

### 🔒 Manejo de JWT
- El JWT es almacenado de forma segura en una **cookie del navegador**.
- Las cookies se envían automáticamente en cada petición al Gateway.
- El token tiene una duración de **60 minutos**, tras lo cual el usuario debe autenticarse de nuevo.

### 🔁 Peticiones al Gateway
El frontend realiza las siguientes peticiones al Gateway:

- **GET** `/files`: Obtiene la lista de archivos del usuario autenticado.
- **POST** `/files/upload`: Sube un nuevo archivo.
- **PUT** `/files/{id}`: Actualiza la metadata de un archivo.
- **PATCH** `/files/{id}`: Modifica parcialmente la información del archivo.
- **DELETE** `/files/{id}`: Elimina un archivo existente.

Todas las peticiones son enviadas al Gateway con el JWT incluido automáticamente en la cookie.

---

## 📦 Docker

Este proyecto está completamente contenerizado con:

- `Dockerfile` para construir la aplicación MVC
- `docker-compose.yml` para orquestar junto al Gateway y API
- Uso de volumen para persistencia de claves de cifrado (`DataProtection-Keys`)

---

## 🧱 Estructura esperada

```
/Web/
 ├── Controllers/
 ├── Views/
 ├── Models/
 ├── Program.cs
 ├── appsettings.json
 ├── .env
 ├── Dockerfile
 └── README.md
```

---

## 🌍 Vista de usuario

- Login y Registro protegidos
- Vista de archivos del usuario autenticado
- Formulario para subir nuevos archivos
- Mensajes de sesión y errores amigables
- Opción de cerrar sesión que elimina la cookie JWT

---

## 📝 Licencia

MIT — Uso libre para fines académicos o empresariales.

---

## 🙌 Autor

**Julian Gonzalez**  
📧 contacto@tucorreo.com  
🔗 [LinkedIn](https://linkedin.com/in/tuusuario)
