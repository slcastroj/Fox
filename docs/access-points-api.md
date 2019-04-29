
## Definición de puntos de acceso y funcionalidades de API

**Caso:** Farmacia Popular
**Autor:** Sergio Castro
**Fecha mod.:** 2019.04.21

### Puntos de acceso
---

Para cada punto de acceso de la API se describirán sus funciones REST (si aplica) y la información esperada y recibida. El formato de los datos de solicitud y respuesta es JSON.

>La URI base para el acceso de datos es _/api/v1_

Para las respuestas se utilizarán solamente los códigos 200 (OK) si la solicitud fue exitosa, 400 (Bad Request) si hubo un problema al recibir o procesar los datos o 403 (Forbidden) si no se pudo verificar los permisos del solicitante.

### Autenticación y autorización
---

Cada usuario puede tener solo uno de los 3 roles registrados; _"Usuario"_, _"Farmaceutico"_ o _"Administrador"_. Para autenticarse se utiliza **RUT** y **contraseña** , el RUT cuyo formato es sin puntos ni guion, máximo 8 caracteres. La respuesta a una solicitud de autenticación es siempre un nuevo token de acceso, con un tiempo de expiración de 1 hora.

Para la autorización se debe adjuntar (cuando corresponda) el token a la solicitud en forma de encabezado. El formato del encabezado de autorización es el siguiente:

>Authorization: Bearer &lt;token&gt;

Todas las solicitudes necesitan autorización a menos que se especifique lo contrario. La autorización es válida para todos los roles a menos que se especifique.

### API
---

#### /usuario/autenticacion

Sin autorización

##### Solicitud

	POST { rut, password }

##### Respuesta

	POST { rut, token, creacion, expiracion }

#### /usuario

GET solo farmacéutico y administrador  
POST solo farmacéutico y administrador

##### Solicitud

	GET ?{ rol?, cantidad? }
	POST { rut, nombre, password, email, fecha_nacimiento, rol }

##### Respuesta

	GET [{ rut, nombre, email, fecha_nacimiento, rol }]
	POST { rut, nombre, email, creacion, rol }

#### /usuario/{rut}

GET solo usuario actual o cualquiera si administrador o farmacéutico
PATCH solo usuario actual o cualquiera si administrador
PUT solo administrador
DELETE solo administrador

##### Solicitud

	GET
	PATCH { nombre, password, email, fecha_nacimiento }
	PUT{ nombre, password, email, fecha_nacimiento, rol }
	DELETE

##### Respuesta

	GET{ rut, nombre, email, fecha_nacimiento }
	PATCH{ rut, nombre, email, fecha_nacimiento, modificacion }
	PUT { rut, nombre, email, fecha_nacimiento, rol, modificacion }
	DELETE { rut, nombre, email, fecha }
---
#### /producto

GET sin autorización  
POST solo administrador

##### Solicitud

	GET ?{ cantidad?, indice?, tipo?, nombre?, tiene_stock?, laboratorio? }
	POST { nombre, descripción, necesita_receta, maximo_semanal, peso_gr, precio_unidad, laboratorio, stock?, tipo } 

##### Respuesta

	GET [{ id, nombre, descripción, necesita_receta, maximo_semanal, peso_gr, precio_unidad, laboratorio, stock, tipo }]
	POST { id, nombre, creacion }

#### /producto/{id}

GET sin autorización
PUT y DELETE solo administrador

##### Solicitud

	GET
	PUT { nombre, descripcion, necesita_receta, maximo_semanal, peso_gr, precio_unidad, laboratorio, stock, tipo }
	DELETE

##### Respuesta

	GET { id, nombre, descripción, necesita_receta, maximo_semanal, peso_gr, precio_unidad, laboratorio, stock, tipo}
	PUT { id, nombre, modificacion }
	DELETE { id, nombre, fecha }
---
#### /reserva

GET solo administrador o farmacéutico

##### Solicitud

	GET ?{ usuario?, producto?, estado? }
	POST { usuario, reservas:[{cantidad, producto}]}

##### Respuesta

	GET [	{ id, usuario, compra, reservas:[{cantidad, producto}]}]
	POST { id, usuario, creacion, reservas:[{cantidad, producto}]}

#### /reserva/{id}

GET y DELETE solo administrador

##### Solicitud

	GET
	DELETE

##### Respuesta

	GET { id, usuario, compra, reservas:[{cantidad, producto}]}
	DELETE { id, fecha }

---
#### /compra

GET solo administrador

##### Solicitud

	GET ?{ usuario?, producto?, estado? }
##### Respuesta

	GET [{ id, estado, reserva:{ id, usuario, reservas:[{cantidad, producto}]}}]

#### /compra/{id}

GET solo de usuario o cualquiera si administrador o farmacéutico
PATCH solo cancelar usuario, cualquiera administrador o farmacéutico
DELETE solo administrador

##### Solicitud

	GET
	PATCH { estado }
	DELETE

##### Respuesta

	GET { id, estado, reserva:{ id, usuario, reservas:[{cantidad, producto}]}}
	PATCH { id, reserva, estado, modificacion }
	DELETE { id, reserva, fecha }
---

**/estadocompra**
**/laboratorio**
**/tipoproducto**

GET sin autorización
POST solo administrador

##### Solicitud

	GET
	POST { nombre }

##### Respuesta

	GET [{ id, nombre }]
	POST { id, nombre, creacion }

**/estadocompra/{id}**
**/laboratorio/{id}**
**/tipoproducto/{id}**

##### Solicitud

	GET
	DELETE

##### Respuesta

	GET { id, nombre }
	DELETE { id, nombre, fecha }