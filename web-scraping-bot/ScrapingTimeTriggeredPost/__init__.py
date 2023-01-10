import datetime
import logging
import requests
from bs4 import BeautifulSoup

import azure.functions as func

def get_from_medium():
    root_url = 'https://medium.com'
    page_url = f'{root_url}/tag/programming'
    req = requests.get(page_url)
    soup = BeautifulSoup(req.text, 'html.parser')
    l = []
    for p in soup.find_all('h2'):
        if p['class'] == ['bd', 'ej', 'kn', 'ko', 'kp', 'kq', 'eo', 'kr', 'ks', 'kt', 'ku', 'es', 'kv', 'kw', 'kx',
                          'ky', 'ew', 'kz', 'la', 'lb', 'lc', 'fa', 'ld', 'le', 'lf', 'lg', 'fe', 'ff', 'fg', 'fh',
                          'fj', 'fk', 'bi']:
            l.append({
                'title': p.get_text(),
                'link': f'{root_url}{p.parent.parent["href"]}'
            })
    return l


def get_from_stackoverflow():
    root_url = 'https://stackoverflow.com'
    page_url = f'{root_url}/questions?tab=Newest'
    req = requests.get(page_url)
    soup = BeautifulSoup(req.text, 'html.parser')
    l = []
    for q in soup.find_all('a', class_='s-link'):
        if q['href'].startswith('/'):
            l.append({
                'title': q.get_text(),
                'link': f'{root_url}{q["href"]}'
            })
    return l


def get_from_sekurak():
    page_url = 'https://sekurak.pl/?cat=7,3'
    req = requests.get(page_url)
    soup = BeautifulSoup(req.text, 'html.parser')
    l = []
    for q in soup.find_all('h2', class_='postTitle'):
        a_tag = q.findNext('a')
        l.append({
            'title': a_tag.get_text(),
            'link': a_tag['href']
        })
    return l


def get_from_zaufana_trzecia_strona():
    page_url = 'https://zaufanatrzeciastrona.pl/'
    req = requests.get(page_url)
    soup = BeautifulSoup(req.text, 'html.parser')
    l = []
    for q in soup.find_all('h2', class_='entry-title'):
        a_tag = q.findNext('a')
        l.append({
            'title': a_tag.get_text(),
            'link': a_tag['href']
        })
    return l

def main(mytimer: func.TimerRequest) -> None:
    utc_timestamp = datetime.datetime.utcnow().replace(
        tzinfo=datetime.timezone.utc).isoformat()

    if mytimer.past_due:
        logging.info('The timer is past due!')

    logging.info('Python timer trigger function ran at %s', utc_timestamp)

    get_methods = [
        get_from_medium,
        get_from_stackoverflow,
        get_from_sekurak,
        get_from_zaufana_trzecia_strona
    ]

    all_data = []

    for f in get_methods:
        all_data.extend(f())

    requests.post(
        'https://azure-xl-api.azurewebsites.net/api/website',
        json=all_data
    )
