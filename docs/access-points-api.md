
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

	GET { roles?, cantidad? }
	POST { rut, password, email, fecha_nacimiento, roles }

##### Respuesta

	GET [{ rut, email, fecha_nacimiento }]
	POST { rut, email, creacion, roles }

#### /usuario/{rut}

GET solo usuario actual o cualquiera si administrador o farmacéutico
PATCH solo usuario actual o cualquiera si administrador
PUT solo administrador
DELETE solo administrador

##### Solicitud

	GET
	PATCH { password, email, fecha_nacimiento }
	PUT{ password, email, fecha_nacimiento, roles }
	DELETE

##### Respuesta

	GET{ rut, email, fecha_nacimiento }
	PATCH{ rut, modificacion, email, fecha_nacimiento }
	PUT { rut, modificacion, email, fecha_nacimiento, roles }
	DELETE { rut, email, fecha }
---
#### /producto

GET sin autorización  
POST solo administrador

##### Solicitud

	GET { cantidad?, indice?, tipo?, nombre?, tiene_stock?, laboratorio? }
	POST { nombre, descripción, necesita_receta, maximo_semanal, peso_gr, precio_unidad, laboratorio, stock?, tipo } 

##### Respuesta

	GET [{ id, nombre, descripción, necesita_receta, maximo_semanal, peso_gr, precio_unidad, laboratorio, stock, tipo, id_laboratorio, id_tipo }]
	POST { id, nombre, creacion }

#### /producto/{id}

GET sin autorización
PUT y DELETE solo administrador

##### Solicitud

	GET
	PUT { nombre, descripcion, necesita_receta, maximo_semanal, peso_gr, precio_unidad, laboratorio, stock, tipo }
	DELETE

##### Respuesta

	GET { id, nombre, descripción, necesita_receta, maximo_semanal, peso_gr, precio_unidad, laboratorio, stock, tipo, id_laboratorio, id_tipo}
	PUT { id, nombre, modificacion }
	DELETE { id, nombre, fecha }
---
#### /notificación

GET solo usuario actual o cualquiera si administrador o farmacéutico
POST solo usuario o cualquiera si administrador o farmacéutico
DELETE solo administrador

##### Solicitud

	GET { tipo?, usuario?, esta_visto? }
	POST { asunto, mensaje, tipo, link?, usuario }
	DELETE { usuario }

##### Respuesta

	GET [{ id, asunto, mensaje, tipo, link, usuario, esta_visto }]
	POST { id, asunto, tipo, usuario, creación }
	DELETE {usuario, fecha, notificaciones_borradas: [{ id, asunto, tipo }]}

#### /notificación/{id}

GET solo para notificaciones del usuario, cualquiera si administrador
PATCH solo notificaciones de usuario, cualquiera si administrador
DELETE solo administrador

##### Solicitud

	GET
	PATCH { esta_visto }
	DELETE

##### Respuesta

	GET { id, asunto, mensaje, tipo, link, usuario, esta_visto }
	PATCH { id, asunto, esta_visto }
	DELETE { id, asunto, tipo, fecha, usuario }
---
#### /compra

GET solo administrador o farmacéutico

##### Solicitud

	GET { usuario?, asignado?, producto?, estado? }
	POST { usuario, cantidad, producto}

##### Respuesta

	GET [{ id, usuario, asignado, cantidad, estado, producto }]
	POST { id, usuario, cantidad, producto, creacion }

#### /compra/{id}

GET solo compras de usuario o cualquiera si administrador o farmacéutico
PATCH solo cancelar usuario, cualquiera administrador o farmacéutico
PUT cualquiera para administrador o farmacéutico
DELETE solo administrador

##### Solicitud

	GET
	PATCH { estado }
	PUT { id, usuario, asignado, cantidad, estado, producto }
	DELETE

##### Respuesta

	GET { id, usuario, asignado, cantidad, estado, producto }
	PATCH { id, modificacion }
	PUT { id, modificacion }
	DELETE { id, fecha }
---
#### /soporte

GET solo administrador
POST sin autorización
DELETE solo administrador

##### Solicitud

	GET { asignado?, consultante?, estado? }
	POST { consultante }
	DELETE { consultante }

##### Respuesta

	GET { id, asignado, consultante, estado }
	POST { id, consultante, creacion }
	DELETE { id, consultante, asignado, fecha }
	
#### /soporte/{id}

GET, PATCH, DELETE solo administrador

##### Solicitud

	GET
	PATCH { asignado?, estado? }
	DELETE

##### Respuesta

	GET { id, asignado, consultante, estado }
	PATCH { id, asignado, estado}
	DELETE { id, consultante }
---
#### /reserva

GET solo administrador

##### Solicitud

	GET { usuario?, producto? }
	POST { usuario, producto}
	DELETE { producto }

##### Respuesta

	GET [{ id, usuario, producto }]
	POST { id, usuario, producto, creacion }
	DELETE { fecha, n_borrados }

#### /reserva/{id}

GET, PUT y DELETE solo administrador

##### Solicitud

	GET
	PUT { usuario, producto }
	DELETE

##### Respuesta

	GET { id, usuario, producto }
	PUT { id, usuario, producto }
	DELETE { id, usuario, producto, fecha }
---

**/estadocompra**
**/estadosoporte**
**/laboratorio**
**/tiponotificacion**
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
**/estadosoporte/{id}**
**/laboratorio/{id}**
**/tiponotificacion/{id}**
**/tipoproducto/{id}**

##### Solicitud

	GET
	DELETE

##### Respuesta

	GET { id, nombre }
	DELETE { id, nombre, fecha }
