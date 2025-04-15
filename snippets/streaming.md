# Streaming

Het kan tijd kosten voordat je een volledig antwoord van een LLM terug krijgt. Vooral bij hele lange antwoorden (zoals de samenvatting van een boek) kan het lijken of je user interface niet meer reageert. Om een betere user experience te creeëren kan je streaming gebruiken. Je krijgt dan woord-voor-woord een antwoord terug.

```ja
const stream = await model.stream("Write an introduction for a book about a colony of tiny hamsters.")
for await (const chunk of stream) {
    console.log(chunk.content)
}
```

<br><bR><br>

## Streaming vanuit Express

In Express gebruiken we normaal res.send (of res.json) om een response te sturen. Dit stuurt dan de headers, content en sluit de verbinding met de client. Je kunt dit zelf ook volledig uitschrijven.

```js
// headers, content, en end in één
res.send("Hello World"); 

// hetzelfde maar dan uitgeschreven
res.setHeader("Content-Type", "text/plain");
res.write("Hello World");
res.end();
```
Dit kan je nu verwerken in je `POST` request:

```js
app.post('/ask', async (req, res) => {
    const prompt = req.body.prompt
    const stream = await model.stream(prompt);
    res.setHeader("Content-Type", "text/plain");
    for await (const chunk of stream) {
        console.log(chunk.content);
        res.write(chunk.content);
    }
    res.end();
}
```
<br><bR><br>

## Frontend

In je frontend moet je je `fetch` aanroep aanpassen zodat je een `stream` kan inladen. Je kan nu de chunks die één voor één binnenkomen, meteen in de browser tonen.

```js
async function askQuestion(e) {
    e.preventDefault()

    const options = {
        method: 'POST',
        mode: 'cors',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ prompt: field.value })
    }

    const response = await fetch("http://localhost:3000/ask", options);
    const reader = response.body.getReader();
    const decoder = new TextDecoder('utf-8');

    while (true) {
        const { value, done } = await reader.read();
        if (done) break;

        const chunk = decoder.decode(value, { stream: true });
        console.log(chunk);
        endresult.innerText += chunk;
    }
}
```

<br><bR><br>


## Links

- https://developer.mozilla.org/en-US/docs/Web/API/Streams_API/Using_readable_streams
