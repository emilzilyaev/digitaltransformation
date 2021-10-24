FROM node:12.8-buster
EXPOSE 3000
WORKDIR /app

# add `/app/node_modules/.bin` to $PATH
ENV PATH /app/node_modules/.bin:$PATH

# setup and install packages
COPY package.json /app
COPY yarn.lock /app

# install packages inside docker container
RUN yarn install --ignore-engines

# copy everything to /app
COPY .. /app
CMD yarn run start