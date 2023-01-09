Przykładowy request do api:
await axios({
      method: 'post',
      url:
'https://az-next-word-prediction.azurewebsites.net/api/word-prediction',
      headers: { 
        'Content-Type': 'application/json'
      },
      data: JSON.stringify({
        "text": JSON.stringify(text)
      })
    }).then((res) => {
      console.log(res.data)
    });
