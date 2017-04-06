#!/bin/sh

# This script is intendet to prepare the project for submission, put it into
#  the final zip-Container and deploy it to a certain server.

REPO=https://github.com/elektm93/traffic_sim.git
SUBZIP=/tmp/Submission.zip
TMPGITPATH=/tmp/traffic_sim

### create doc

# doxygen
# doxygen src/doxygenConfig

# latex # TODO: twice??
cd doc; pdflatex Documentation.tex
cd doc; pdflatex Documentation.tex
cd doc; pdflatex Documentation.tex

### put everything together into an zip container
git clone $REPO $TMPGITPATH

# doc
cd doc; zip $SUBZIP -r travis_built.pdf

# src
cd /tmp/traffic_sim; zip $SUBZIP -r src/

# vid
## TODO



### "deploy"