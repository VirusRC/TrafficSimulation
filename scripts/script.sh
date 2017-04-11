#!/bin/sh

# This script is intendet to prepare the project for submission, put it into
#  the final zip-Container and deploy it to a certain server.

REPO=https://github.com/schoerg/TrafficSimulation.git
SUBZIP=/tmp/Submission.zip
TMPGITPATH=/tmp/TrafficSimulation


### create doc

# doxygen
# doxygen src/doxygenConfig

git clone $REPO $TMPGITPATH

# latex # TODO: twice??
cd $TMPGITPATH/doc
pdflatex Documentation.tex
pdflatex Documentation.tex
pdflatex Documentation.tex

### put everything together into an zip container


# doc
cd $TMPGITPATH/doc; zip $SUBZIP -r travis_built.pdf

# src
cd /tmp/TrafficSimulation; zip $SUBZIP -r src/

# vid
## TODO



### "deploy"
