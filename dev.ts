import { exec, spawn } from "node:child_process";
import { platform } from "node:os";
import { resolve } from "node:path";

type OS = NodeJS.Platform;

interface ISistemas {
  sistema: OS; // Seria el sistema operativo
  cmd: string; // Seria el comando que se va usar para abrir el navegador con la URL
}

const SISTEMAS: ISistemas[] = [
  {
    sistema: "linux",
    cmd: "xdg-open",
  },
  {
    sistema: "win32",
    cmd: "start",
  },
  {
    sistema: "darwin",
    cmd: "open",
  },
];

const abrirNavegador = (url: string): void => {
  const sistema = SISTEMAS.find((s) => s.sistema === platform());

  if (!sistema)
    throw Error(
      `No se soporta este sistema(${platform()}), use linux mejor compa`,
    );

  const URL = `${url}?ts=${Date.now()}`;
  exec(`${sistema.cmd} ${URL}`);
};

const ROOT_DIR: string = import.meta.dirname;
const FRONT_DIR: string = resolve(ROOT_DIR, "frontend");
const BACK_DIR: string = resolve(ROOT_DIR, "backend");

const correrProcesos = (
  cmd: string, // El comando main
  agrs: string[], // Los argumentos que va tomar el comando por ejemplo => main + args => dotnet watch run
  prefijo: string, // Seria lo que va mostrar en la consola
  dir: string, // El directorio donde va realizar un cwd para ejecutar el comando
  isListo?: (linea: string) => void,
): void => {
  const proceso = spawn(cmd, agrs, { cwd: dir });

  proceso.stdout.on("data", (data: Buffer) => {
    console.log(`NORMAL: [${prefijo.toUpperCase()}] ${data.toString()}`);
    if (isListo) isListo(data.toString());
  });

  proceso.stderr.on("data", (data: Buffer) => {
    console.error(`ERROR: ${prefijo.toUpperCase()} ${data.toString()}`);
  });
};

const FRONT_PORT = 4200;
// const BACK_PORT = 5174;

correrProcesos(
  "pnpm",
  ["run", "ng", "serve"],
  "angular",
  FRONT_DIR,
  (linea: string): void => {
    if (linea.includes("Local:"))
      abrirNavegador(`http:localhost:${FRONT_PORT}`);
  },
);

correrProcesos("dotnet", ["watch", "run"], "dotnet", BACK_DIR);

// TODO: Investigar como abrir una nueva pestaña para que no choquen lo de Angular y Dontet con Scalar en el navegador xd
