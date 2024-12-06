# Function calling

Een taalmodel heeft geen kennis van recent nieuws, sportuitslagen, het weer, of andere hele specifieke recente informatie. Ook is een taalmodel niet altijd goed in berekeningen en kan een taalmodel niet uit zichzelf een game of een smarthome besturen.

Dit kan je toevoegen door je eigen `functions` te schrijven en die functies toe te voegen aan het taalmodel. Het taalmodel kan nu aan de hand van het user prompt zelf bedenken of een van de functies aangeroepen moet worden! In onderstaande voorbeeld schrijven we een functie (tool) die twee nummers vermenigvuldigt. Dit kan je in een prompt aanroepen.

<br><br><br>

## Tool 

Het verschil tussen een gewone `javascript function` en een `tool function` is dat je een *schema* moet meegeven waar precies in staat welke variabelen verwacht worden in de functie. 

```js
import { ChatOpenAI } from "@langchain/openai"
import { tool } from "@langchain/core/tools";

// schrijf hier je javascript functie
const multiplyFunction = ({ a, b }) => {
    return a * b;
};

// Maak een tool van de functie, inclusief het schema
const multiply = tool(multiplyFunction, {
    name: "multiply",
    description: "Multiply two numbers",
    schema: {
        type: "object",
        properties: {
            a: { type: "number" },
            b: { type: "number" },
        },
        required: ["a", "b"],
    },
});

// Roep de tool zelf aan om te testen of het werkt
let resultA = await multiply.invoke({ a: 3, b: 4 });
console.log(resultA)
```
<br>

## Functie aan model koppelen

Via `bindTools` kan je aan het model duidelijk maken welke tools je geschreven hebt, je kan meerdere tools tegelijk doorgeven.

```js
const model = new ChatOpenAI({
    azureOpenAIApiKey: process.env.AZURE_OPENAI_API_KEY,
    azureOpenAIApiVersion: process.env.OPENAI_API_VERSION,
    azureOpenAIApiInstanceName: process.env.INSTANCE_NAME,
    azureOpenAIApiDeploymentName: process.env.ENGINE_NAME,
})
const modelWithTools = model.bindTools([multiply]);
```
<br>

## Taalmodel roept functie aan

Het model bepaalt zelf wanneer de tool aangeroepen moet worden, dit kan je als volgt testen:

```js
// test if the model works normally
const resultB = await modelWithTools.invoke("How do I say goodbye in Japanese?");
console.log(resultB.content)

// Invoke the model with the tool
const resultC = await modelWithTools.invoke("What is 13 multiplied by 34? You dont have to explain any code, just give the result directly.");
console.log(resultC.content);
```
<br><br><br>

## Todo

Je kan function calling gebruiken om weerdata of sportdata op te halen, of wellicht ook om muziek af te spelen of een game te besturen.

<br><br><br>

## Links

- [Building Langchain tools](https://js.langchain.com/docs/concepts/tools/)
- [Calling Langchain tools](https://js.langchain.com/docs/concepts/tool_calling/)
- [OpenAI detailed info on function calling](https://platform.openai.com/docs/guides/function-calling?lang=node.js)
