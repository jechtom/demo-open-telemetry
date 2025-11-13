# Use the official Node.js 22 image as base
FROM node:22-alpine AS build

# Set the working directory inside the container
WORKDIR /app

# Copy package.json and package-lock.json (if available)
COPY package*.json ./

# Install dependencies
RUN npm ci --only=production

# Copy the source code
COPY . .

# Compile TypeScript to JavaScript
RUN npx tsc

# Production stage
FROM node:22-alpine AS production

WORKDIR /app

# Copy package.json for production dependencies
COPY package*.json ./

# Install only production dependencies
RUN npm ci --only=production && npm cache clean --force

# Copy the compiled JavaScript from build stage
COPY --from=build /app/dist ./dist

# Expose the port the app runs on
EXPOSE 8080

# Run the application
CMD ["node", "--require", "./dist/instrumentation.js", "./dist/app.js"]