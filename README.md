# The Faff Project
Can we build a .net 6 app on Docker and also GitHub Actions

This is a single file app written in .NET 6 beta something or other and using ASP.NET Core 6 beta somethingorother. 
Since you probably don't want beta bits on your local machine, you can execute `.\build-docker` to get a tagged image called `faff`.
You can then execute `.\run-docker` to start a container with that tagged image on port 8000 in interactive mode (`ctrl+c` to kill it).

If you want a persistent database then you will want to pass in a volume mount for `/app/data`. My "prod" docker-compose does this and you can run `docker-compose -f .\docker-compose.dev.yaml up --build` 
to start a docker container on port 5020. The LiteDB database will be saved to the `db` host volume mount.

## Prerequisites

The first time you run an ASP.NET Core application locally, you'll want to install the dev cert. You can execute the following command from your favorite command prompt

```powershell
dotnet dev-certs https --trust
```

This will present you with a dialog asking if you're really sure that you want to do it. Yes, you do so click `yes`.

Then, once you run the app and navigate to the site, you'll likely get a browser warning asking you if you're really sure that you want to proceed because the certificate is self-signed. While this does raise
a philosophical question on if someone can ever truly trust themselves, that doesn't help us with moving forward so go into the appropriate section on the browser warning and tell it to trust the certificate.

## Where This Project is Going

This is a playground for a few things. The first was just to try and get a GitHub Action with .NET 6.0 to run unit tests during the build. Then I added Docker. Then I decided it needed a UI. And I decided to lean on Tailwind for the CSS. Then I decided to use Razor because that plays nicely with Tailwind for CSS purging to remove unused styles.

So, from here, I'm going to add some basic HTML flow because I want to bring in [HTMX](https://htmx.org) to play with. And I want to add in some real tests and some integration tests.

Oh, and of course I need to bring in LiteDB since I mentioned it in the first section. And save stuff to it. Fun stuff.

## Special Thanks

This little app is made possible by:
- Favicon is from [iconpacks.net](https://www.iconpacks.net)
