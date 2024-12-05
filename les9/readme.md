# les 9

## Verbeteren chatbot

Deze les bevat onderwerpen voor het expert level van de opdracht.

- Externe API's toevoegen zoals het weerbericht. Het LLM gebruiken om daar iets over te zeggen.
- Serverless functies voor live hosting op sites zoals Vercel.com.
- Spraak en spraakherkenning van de browser
- Spraak van OpenAI
- Een LLM gebruiken als OS, bv. om een robot te besturen.

<br>

## Externe API's toevoegen

Een LLM bevat geen live informatie over bijvoorbeeld het *recente nieuws, het weer, sportuitslagen, etc.* Als je dus aan je taalmodel vraagt of je een paraplu mee moet nemen naar buiten, dan krijg je geen goed antwoord.

Dit kan je oplossen door zelf een API call te doen en het resultaat daarvan door het LLM te laten uitleggen. Je kan bv. de JSON van een weer-app aan een LLM geven en vragen om kledingadvies te geven. 

### Function calling en JSON

Je kan detecteren of een gebruiker vraagt om bepaalde informatie, waarna je het LLM zelf een functie laat uitvoeren. Bv. als de gebruiker praat over het weer, dan weet het taalmodel dat de "call weather api" functie uitgevoerd moet worden. Dit heet "***Tool / function calling***"

- [Tool / Function calling with Langchain](https://js.langchain.com/docs/how_to/tool_calling/)

<br><br><br>

## Serverless functies

Je kan je project live zetten op een webserver, als die webserver `node` ondersteunt en ook continu de `node server` kan draaien. Dit kan je doen op je HR studentenhosting. Het nadeel is dat je zelf moet monitoren wanneer de server is uitgevallen.

Er zijn ook online ***node hosting*** providers te vinden zoals `vercel.com`, `netlify.com`, `codesandbox.com`, `github codespaces`, `huggingface spaces`, `stackblitz.com`, `deno.com`, `amazon serverless webservices`. Deze maken vaak gebruik van ***serverless functions*** zodat er geen live node server hoeft te draaien. 

### Voorbeeld

- TODO

<br><br><br>

## Browser spraak

- TODO

<br><br><br>

## OpenAI spraak

- TODO
