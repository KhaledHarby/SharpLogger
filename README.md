
## Register File Logger 
   SharpLogger.LoggerHandlerManager.RegisterLogger(new SharpFileLogger());
   SharpLogger.LoggerHandlerManager.RegisterLogger(new SharpFileLogger("fileName"));
   SharpLogger.LoggerHandlerManager.RegisterLogger(new SharpFileLogger("fileName","directory"));
   
   
## Register Database Logger 
   SharpLogger.LoggerHandlerManager.RegisterLogger(new SharpDbLogger(DatabaseType.SqlServer, "SqlServerConnectionString")); <br />
   SharpLogger.LoggerHandlerManager.RegisterLogger(new SharpDbLogger(DatabaseType.MySql, "MySqlConnectionString")); <br />
   SharpLogger.LoggerHandlerManager.RegisterLogger(new SharpDbLogger(DatabaseType.Oracle, "OracleConnectionString")); <br />
   
   
   
## Register Email Logger

      SharpLogger.LoggerHandlerManager.RegisterLogger(EmailLogger());
	  
      static SharpEmailLogger EmailLogger()
        {
            EmailConfiguration emailConfiguration = new EmailConfiguration("userName", "Password", "host", 456, true, false);
            SharpEmailLogger sharpEmailLogger = new SharpEmailLogger(emailConfiguration);
            sharpEmailLogger.AddRecipient("example1@site.com");
            sharpEmailLogger.AddRecipient("example2@site.com");
            sharpEmailLogger.AddRecipient("example3@site.com");

            return sharpEmailLogger;
        }  


## Register Queue Logger
     SharpLogger.LoggerHandlerManager.RegisterLogger(new SharpQueueLogger());
		

Example
-------
```csharp
    class Program
    {
        static void Main(string[] args)
        {
            // Register Handler
            SharpLogger.LoggerHandlerManager.RegisterLogger(new SharpFileLogger());
            SharpLogger.LoggerHandlerManager.RegisterLogger(new SharpDbLogger(DatabaseType.SqlServer,"SqlServerConnectionString"));
            SharpLogger.LoggerHandlerManager.RegisterLogger(new SharpDbLogger(DatabaseType.MySql, "MySqlConnectionString"));
            SharpLogger.LoggerHandlerManager.RegisterLogger(new SharpQueueLogger());
            SharpLogger.LoggerHandlerManager.RegisterLogger(EmailLogger());

			
			
			// Remove Handler
			SharpLogger.LoggerHandlerManager.RemoveLogger(sharpFileLogger());
			
			
			

            // Log Exception 
            try
            {
                bool.Parse("66");
            }
            catch (Exception ex)
            {
                SharpLogger.Log(ex);
            }
			
			
			
           try
            {
                throw new Exception("new exception");
            }
            catch (Exception ex)
            {
                SharpLogger.Log(ex);
            }



            // Log Message  
            SharpLogger.Log("Message");



            // Log message - entry level
            SharpLogger.Log(SharpLogger.Level.None, "message");
            SharpLogger.Log(SharpLogger.Level.Info, "message");
            SharpLogger.Log(SharpLogger.Level.Warning, "message");
            SharpLogger.Log(SharpLogger.Level.Error, "message");
            

			
			
			 // Using Threads 
            Thread thread1=new Thread(() => {

                for (int i = 0; i < 1000; i++)
                {
                    SharpLogger.Log(string.Format("Message {0}", i));					
                }

            });

        }


        static SharpFileLogger sharpFileLogger()
        {
            return new SharpFileLogger();
        }

        static SharpDbLogger SqlServerLogger()
        {
            return new SharpDbLogger(DatabaseType.SqlServer, "SqlServerConnectionString");
        }

        static SharpDbLogger MySqlLogger()
        {
            return new SharpDbLogger(DatabaseType.MySql, "MySqlConnectionString");
        }

        static SharpDbLogger OracleLogger()
        {
            return new SharpDbLogger(DatabaseType.Oracle, "OracleConnectionString");
        }

        static SharpEmailLogger EmailLogger()
        {
            EmailConfiguration emailConfiguration = new EmailConfiguration("userName", "Password", "host", 456, true, false);
            SharpEmailLogger sharpEmailLogger = new SharpEmailLogger(emailConfiguration);
            sharpEmailLogger.AddRecipient("example1@site.com");
            sharpEmailLogger.AddRecipient("example2@site.com");
            sharpEmailLogger.AddRecipient("example3@site.com");

            return sharpEmailLogger;
        }

        static SharpQueueLogger QueueLogger()
        {
            return new SharpQueueLogger();
        }
    }
```

