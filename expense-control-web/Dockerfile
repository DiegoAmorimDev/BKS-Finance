FROM node:lts-alpine

WORKDIR /app

# Copia os arquivos de definição de pacotes
COPY package*.json ./

# Instala as dependências
RUN npm install

# Copia o restante do código
COPY . .

# Expõe a porta padrão do Vite
EXPOSE 5173

# Inicia o servidor de desenvolvimento com --host para permitir acesso externo ao container
CMD ["npm", "run", "dev", "--", "--host"]
