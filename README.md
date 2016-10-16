# Panda
Entity Driven API Engine, using an annotation rules engine.

# Idea
As we create a ton of entities and then wire them to Repositories followed by an API controller which serves the rest calls.
What and how many of these steps can we skip.
If we had an Entity as follows:

    public class Game : Panda.IPandaEntity
    {
        public string Name { get; set; }
    }
    
Could we then call directly in to it using some clever reflection?

Now one might ask, what about business rules and authentication?
well, such things could be added via Attributes. making it a Model-Annotation Driven API.

    [Auth(IMyAuthentication)]
    public class Game : Panda.IPandaEntity
    {
        public string Name { get; set; }
    }
    
Which could potentially check for authentication before DB query or any other rules.

# Future
This is the first step. Next up is looking at Database migrations.
currently we generate an SQL Table for the entities that inherite IPandaEntity.
With the tables created we can POST to http://localhost/api/game/ with some json data.
or if we make a GET request to http://localhost/api/game/ we get back what we have posted.
meaning we skip the controller. well in a way.

* Better Database migration.
* Basic Rules Engine (Annotation driven behavior)
* Foreign References
* Unit Tests (way down the line on my todo list, but you are welcome to aid in this)

When I say rule engine what that could be:
* Field Requirements, such as Entity Requires all fields (dont accept partial objects)
* Custom or ASP.NET Identity authentication (as an example)

Custom Attributes could be:
* API Throttle Limit Attribute.

# Contribute
This might have been an overly ambitious project to start at 2 in the morning. so if you find this to be handy, 
please contribute with pull requests.
