Modify the RepositoryGenerator.tt file, replace the line:

string targetNamespace = "(ENTER THE FULL NAMESPACE TO YOUR MODELS)";

with your the fully qualified namespace to your entity framework models. If your DbContext class
is not in this namespace please also modify this line:

string efContext = "DefaultModel";

replace DefaultModel with the name of the class that implements the DbContext class.

///////////////////////////////////////////////////////////////////////////////////////////////

All your repositories will inherit from Repository<T> which contains generic methods, feel free
to override any methods. To add new repository methods, please add them to the I(ClassName)Repository
interface and implement them in the (ClassName)Repository.