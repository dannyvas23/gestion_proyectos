// Configuración de producción (Docker)
// Las URLs apuntan al backend dentro de la red Docker via Nginx reverse proxy.
export const environment = {
  production: true,
  apiUrl: '/api',
  hubUrl: '/hub/tablero'
};
