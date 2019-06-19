#!/usr/bin/env node

const express = require("express");
const fs = require("fs-extra");
const fsPath = require("path");
const options = require("yargs")
  .options({
    port: {
      type: "number",
      default: 3000,
      describe: "port to user"
    },
    baseDir: {
      type: "string",
      default: ".",
      describe: "directory base"
    }
  })
  .help().argv;

const app = express();

const getJson = async (path, verb) => {
  let paths = [`${path}.${verb}.json`, `${path}/${verb}.json`];

  if (verb === "get") {
    paths = paths.concat([`${path}.json`, `${path}/index.json`]);
  }

  paths = paths.map(p => fsPath.join(options.baseDir, p));
  let match;
  for (const path of paths) {
    const exists = await fs.exists(path);
    if (exists) {
      match = path;
      break;
    }
  }

  if (match) {
    const json = await fs.readFile(match);
    return json;
  }

  throw { message: "json not found" };
};

app.all("/api*", (req, res) => {
  const verb = req.method.toLowerCase();
  const path = req.path
    .split("/")
    .filter(s => s.trim())
    .splice(1)
    .join("/");

  res.contentType("application/json");

  getJson(path, verb)
    .then(json => {
      res.send(json);
    })
    .catch(err => {
      res.status(404);
      res.send(err);
    });
});

app.listen(options.port, () =>
  console.log(
    `listening on port: ${options.port}\nbase directory: ${options.baseDir}`
  )
);