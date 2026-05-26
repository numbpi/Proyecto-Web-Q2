using Google.Cloud.Firestore;

namespace Proyecto_Web_Q2.Services;

public class FireBaseService
{
    private readonly FirestoreDb _firestoreDb;
    protected string projectId = "proyecto-web-grupo6";

    public FireBaseService()
    {
        // Aca le decimos al FB donde debe buscar nuestras credentiales y el Path.Combine() hace que busque en nuestra Solución o Projecto que lo busque en la carpeta Config y pasarle el archivo
        var credentialPath = Path.Combine(
            AppContext.BaseDirectory,
            "Config",
             "firebase-credentials.json"
        );

        // Seteamos la variable de entorno para que el SDK de Firebase pueda encontrar las credenciales
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);

        // Aca iniciamos el Firestore mediante el projectId, que se encuentra en el archivo de las crendenciales => project_id
        _firestoreDb = FirestoreDb.Create(projectId);
    }

    /*
      Va devolver una referencia de una X colleción y usamos un lamba para que se vea mas perron xd.
      Porque el lamba asi ya un return.
     */
    public CollectionReference GetCollection(string colletionName) =>
        _firestoreDb.Collection(colletionName);
}
