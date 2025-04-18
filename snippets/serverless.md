# Serverless

Er zijn online ***node hosting*** providers zoals[vercel.com](https://vercel.com), [netlify.com](https://netlify.com), [render.com](https://render.com), [codesandbox.com](https://codesandbox.com), [github codespaces](https://github.com/features/codespaces), [huggingface spaces](https://huggingface.co/spaces), [stackblitz.com](https://stackblitz.com), [deno.com](https://deno.com), [amazon serverless webservices](https://aws.amazon.com/serverless/), etc. Deze maken gebruik van ***serverless functions*** zodat er geen live node server hoeft te draaien. 

Deze maken gebruik van ***serverless functions*** zodat er geen live node server hoeft te draaien. 

Om hiermee te werken moet je je node code omzetten naar `pure functions`: functies die een waarde verwachten en een waarde teruggeven. Er zijn geen *global variables* of *state* beschikbaar in een serverless app.

<br><br><br>

### Vercel

- Maak een nieuw project op de Vercel omgeving. Volg de [getting started instructies](https://vercel.com/docs/getting-started-with-vercel/template)
- Kies het [meest simpele starter template: nodejs hello world](https://vercel.com/templates/other/nodejs-serverless-function-express).
- Link Vercel met jouw github. Dit zorgt ervoor dat een push naar jouw github automatisch op Vercel wordt gedeployed. 
- De packages uit jouw `package.json` worden automatisch in je vercel project geinstalleerd.
- Binnen de vercel omgeving moet je `env` variabelen aanmaken. Dit staat dus niet in een `.env` file *(deployment > settings > environment variables)*.

### Project

Lokaal ziet je project er als volgt uit. 

```
PROJECT FOLDER
├── index.html
├── index.js
├── package.json
└── API
      └── hello.js
```

#### Serverless function

In `api/hello.js` plaats je de serverless code:

```js
import type { VercelRequest, VercelResponse } from '@vercel/node'

export default function handler(req, res) {
  const { name = 'World' } = req.query
  return res.json({
    message: `Hello ${name}!`,
  })
}
```

#### Frontend

Vanuit `index.js` kan je de serverless function aanroepen:

```js
async function fetchGreeting(name) {
  try {
    const response = await fetch(`/api/hello?name=${name}`);
    const data = await response.json();
    console.log(data.message); // "Hello Action Henk!" 
  } catch (error) {
    console.error('Error fetching the API:', error);
  }
}

fetchGreeting('Action Henk');
```

<br><br><br>

#### Vectorstores

Omdat een `serverless` omgeving stateless is, kan je geen FAISS vectorstore lokaal inladen, om daar dan later vragen aan te kunnen stellen. Je kan wel voor iedere prompt een call naar een online vectordatabase doen, bijvoorbeeld [PineCone](https://www.pinecone.io). Daar moet je dan ook weer een API key voor aanmaken.

- [Pinecone Code Snippet](https://github.com/HR-CMGT/PRG08-2024-2025/blob/main/snippets/pinecone.md)

<br><br><br>

#### CORS

In dit geval staan de frontend en de serverless code op dezelfde server. Als je de server vanuit een ander domein wil kunnen aanroepen moet je `cors` aanzetten op de server.

<br><br><br>


## Links

- [Hello World Serverless](https://vercel.com/templates/other/nodejs-serverless-function-express)
- [Vercel Serverless documentation](https://vercel.com/docs/functions)
