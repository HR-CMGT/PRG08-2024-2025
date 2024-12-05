# Les 6 (deel 1)

## Webproject opzetten

- Zorg dat je [NodeJS 22](https://nodejs.org/en) hebt geïnstalleerd.
- Installeer [JSONFormatter](https://chromewebstore.google.com/detail/json-formatter/bcjindcccaagfpapjjmafapmmgkkhgoa) of een andere browser extensie waarmee je JSON kan bekijken.
- Maak een nieuw project aan met de volgende structuur:

```
SERVER
├── .gitignore
├── server.js
CLIENT
├── index.html
├── script.js
└── style.css
```

<br><br><br>

## Server

Maak je `.gitignore` file als volgt aan:

```sh
.vscode
.idea
.env
node_modules
```

<br><br><br>

## Express

In PRG06 heb je geleerd te werken met node express. Als herhalingsoefening gaan we een server maken die je vanuit je HTML pagina kan aanroepen. Start het `npm` project in je `server` folder:

```sh
cd server
npm init
npm install express
npm install cors
npm install body-parser
```
Je kan nu een `server.js` file aanmaken waarin we de `express` server kunnen opstarten.

```js
import express from 'express'
import cors from 'cors'

const app = express()
app.use(cors())

app.get('/', (req, res) => {
  res.json({ message: 'Hello, world!' })
})

app.listen(3000, () => console.log(`Server running on http://localhost:3000`))
```

<br><br><br>

## Server testen

Start de server!

```sh
node server.js
```
> ⚠️ *TIP Om te voorkomen dat je de server telkens opnieuw moet opstarten als je iets aanpast kan je ook `node --watch server.js` gebruiken*

Roep de server aan in je adresbalk van je browser of via [Postman](https://www.postman.com), [Hoppscotch](https://hoppscotch.io) of [Thunder Client](https://www.thunderclient.com)

http://localhost:3000

<br><br><br>

## Client

- Maak je `index.html`, `style.css` en `app.js` files aan in de `client` map.
- Je hoeft hier geen `npm install` te doen omdat we niet met libraries werken.
- Open dit als apart project in je code editor. Je kan een `live server` starten voor `index.html`.

### Testen met Fetch

Om te oefenen gaan we de server aanroepen met `fetch` vanuit `app.js`. Kijk of het bericht `hello world` verschijnt in de console (in de client).

```js
async function askQuestion(e) {
    e.preventDefault()
    const response = await fetch("http://localhost:3000/") 
    if(response.ok){
        const data = await response.json()
        console.log(data)
    } else {
        console.error(response.status)
    }
}
```
#### Oefening

Toon het resultaat in de HTML pagina in plaats van in de console.

<br><br><br>

### Formulier maken

Het is de bedoeling dat de gebruiker een vraag kan sturen naar je server. Dit gaan we doen met een POST request.

- Maak een invulformulier met button in de `html` file met een submit button.
- On submit roep je je fetch function aan. Let op dat je de default submit actie annuleert.

```html
<form>
    <input type="text" placeholder="je vraag hier" id="chatfield">
    <button type="submit">
</form>
```
```js
const form = document.querySelector("form")
form.addEventListener("submit", (e) => askQuestion(e))

function askQuestion(e){
    e.preventDefault()
}
```

<br><br><br>

### POST request sturen

We gaan de `fetch` functie uitbreiden met een `POST` request waarmee de waarde uit het invulveld wordt verstuurd.

```js
async function askQuestion(e) {
    e.preventDefault()

    const options = {
        method: 'POST',
        mode:'cors',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ prompt: chatfield.value })
    }

    const response = await fetch("http://localhost:3000/", options) 
    if(response.ok){
        const data = await response.json()
        console.log(data)
    } else {
        console.error(response.status)
    }
}
```

<br><br><br>

### POST request lezen

Deze data komt nu binnen in de server, dit kan je ook weer testen. In dit voorbeeld geven we de vraag gewoon weer terug als resultaat.

```js
const app = express()
app.use(cors())
app.use(bodyParser.json())
app.use(bodyParser.urlencoded({ extended: false }))

app.post('/', async (req, res) => {
    let prompt = req.body.prompt
    res.json({ message: 'Hello, world!', originalquestion: prompt })
})

app.listen(3000, () => console.log(`app luistert naar port 3000!`))
```
<br><br><br>

### Formulier af maken

Je hebt nu de basics van een node server weer up and running! Je kan het formulier nog verbeteren:

- Zorg dat je submit button disabled is zo lang er nog geen antwoord terug is gekomen. Toon via een `loading spinner` dat de app bezig is.
- Het resultaat toon je vervolgens weer aan de gebruiker in de user interface. Enable de submit button en verwijder de loading spinner.

Hierna gaan we verder met het toevoegen van [langchain](langchain.md)

<br><Br><br>

## Links

- [Node Express Hello World](https://expressjs.com/en/starter/hello-world.html)
- [JSON teruggeven vanuit Express](https://expressjs.com/en/5x/api.html#res.json)
- [Voorbeeld fetch met POST](https://jasonwatmore.com/post/2021/09/05/fetch-http-post-request-examples)
