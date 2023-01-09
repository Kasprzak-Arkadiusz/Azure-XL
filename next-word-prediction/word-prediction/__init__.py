import logging

import azure.functions as func

from transformers import OpenAIGPTTokenizer, pipeline

tokenizer = OpenAIGPTTokenizer.from_pretrained('openai-gpt')
generator = pipeline('text-generation', model='openai-gpt')

def main(req: func.HttpRequest) -> func.HttpResponse:
    text = req.params.get('text')
    
    if not text:
        try:
            req_body = req.get_json()
        except ValueError:
            pass
        else:
            text = req_body.get('text')

    if text:
        length = len(tokenizer.encode(text, return_tensors='pt')[0])
        max_length = length + 1
        
        return func.HttpResponse(
            generator(text, max_length=max_length, do_sample=True, temperature=0.7)[0]['generated_text'].split(' ')[-1],
            status_code=200
        )
    else:
        return func.HttpResponse(
             "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response.",
             status_code=200
        )
