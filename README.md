Para el envío de correo utilice el SMTP de Gmail. Tuve que generar un app password desde mi cuenta de gmail para poder utilizar el servicio (https://myaccount.google.com/apppasswords)

En el appsettings se pueden configurar las credenciales para utilizar otro cliente (Outlook, Gmail)

"Smtp": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Username": "",
    "Password": "",
    "EnableSsl": true,
    "From": ""
  }

  En el archivo Index.cshtml incluí un script para llamar al endpoint de /email/send. La funcion se llama sendEmail(). Es necesario tambien cambiar la propiedad "to" para indicar a quien deben llegar los correos

  
