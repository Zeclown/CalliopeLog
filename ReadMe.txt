Calliope Log is a library that allows multiple log categories and verbosity levels
To use the library, simply press on the Calliope Log Manager Option under Tools->Calliope.
To use the Calliope Logging in code, use Calliope.Debug.X exactly like the Unity debug log API. (EG. Calliope.Debug.Log((int)Calliope.LogCategory.AI, (int)Calliope.LogVerbosity.Important, "Important ok?");)
Extend Calliope.LogCategory and Calliope.LogVerbosity to fit your needs.