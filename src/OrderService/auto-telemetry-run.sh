#!/bin/bash
set -e

# Setup the instrumentation for the current shell session
. $HOME/.otel-dotnet-auto/instrument.sh

# Run the application
dotnet OTelDemo.Backend.dll
