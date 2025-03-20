# Serverless

Er zijn online ***node hosting*** providers zoals[vercel.com](https://vercel.com), [netlify.com](https://netlify.com), [render.com](https://render.com), [codesandbox.com](https://codesandbox.com), [github codespaces](https://github.com/features/codespaces), [huggingface spaces](https://huggingface.co/spaces), [stackblitz.com](https://stackblitz.com), [deno.com](https://deno.com), [amazon serverless webservices](https://aws.amazon.com/serverless/), etc. Deze maken gebruik van ***serverless functions*** zodat er geen live node server hoeft te draaien. 

Deze maken gebruik van ***serverless functions*** zodat er geen live node server hoeft te draaien. 

Om hiermee te werken moet je je node code omzetten naar `pure functions`: functies die een waarde verwachten en een waarde teruggeven. Er zijn geen *global variables* of *state* beschikbaar in een serverless app.

<br><br><br>

### Voorbeeld

Maak een vercel project als volgt aan:

```
PROJECT FOLDER
├── index.html
├── index.js
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

#### CORS

In dit geval staan de frontend en de serverless code op dezelfde server. Als je de server vanuit een ander domein wil kunnen aanroepen moet je `cors` aanzetten op de server.

<br><br><br>


## Links

- [Hello World Serverless](https://vercel.com/templates/other/nodejs-serverless-function-express)
- [Vercel Serverless documentation](https://vercel.com/docs/functions)