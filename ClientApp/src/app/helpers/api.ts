import { environment } from 'src/environments/environment';
const { baseUrl } = environment;
export const apiPost = <T>(url: string, data: any): Promise<T> =>
  fetch(baseUrl + url, {
    method: 'POST', // *GET, POST, PUT, DELETE, etc.
    mode: 'cors', // no-cors, cors, *same-origin
    cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
    credentials: 'include', // include, *same-origin, omit
    headers: {
      'Content-Type': 'application/json'
      // "Content-Type": "application/x-www-form-urlencoded",
    },
    redirect: 'follow', // manual, *follow, error
    referrer: 'no-referrer', // no-referrer, *client
    body: JSON.stringify(data) // body data type must match "Content-Type" header
  }).then(res => {
    if (res.status !== 200) {
      throw res;
    }
    try {
      return res.json().catch(_ => undefined);
    } catch {
      return Promise.resolve();
    }
  });

export const apiGet = <T>(url: string): Promise<T> =>
  fetch(baseUrl + url, { credentials: 'include' }).then(res => {
    if (res.status !== 200) {
      throw res;
    }
    return res.json();
  });

export interface Request {
  url: string;
  baseUrl?: string;
  method: 'GET' | 'POST' | 'PUT';
  body: string;
  headers: { [key: string]: string };
  withCredetials: boolean;
  async: boolean;
}

const sendRequest = (request: Request) => {
  return new Promise<any>(function(resolve, reject) {
    const xhr = new XMLHttpRequest();
    const url = request.baseUrl ? request.baseUrl + request.url : request.url;
    xhr.open(request.method, url, request.async);
    xhr.withCredentials = true;

    Object.keys(request.headers).forEach(key => {
      xhr.setRequestHeader(key, request.headers[key]);
    });

    xhr.onload = function() {
      if (this.status >= 200 && this.status < 300) {
        resolve(xhr.response);
      } else {
        reject({
          status: this.status,
          statusText: xhr.statusText
        });
      }
    };
    xhr.onerror = function() {
      reject({
        status: this.status,
        statusText: xhr.statusText
      });
    };
    xhr.send(request.body);
  });
};

const withDefault = def => req => Object.assign(def, req);

const defaultOptions: Partial<Request> = {
  baseUrl: environment.baseUrl,
  headers: {
    'Content-Type': 'application/json'
  },
  withCredetials: true,
  async: true
};

type MakeRequest = (req: Partial<Request>) => Request;
const makeRequest: MakeRequest = withDefault(defaultOptions);

export const get = <T>(url: string, options: Partial<Request> = {}) =>
  sendRequest(makeRequest({ ...options, url, method: 'GET' })).then<T>(
    JSON.parse
  );
