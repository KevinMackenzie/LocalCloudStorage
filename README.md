# LocalCloudStorage (That's a boring name!)
## Important
I've lost inspiration for this project for the time being.  I may resume work on it in the future.
## More Info
You can find the below text along with a lot more planning information [here](https://docs.google.com/document/d/12du6jhgV2ifR7IV8nfTaC20sdfsx8A39oGmF1N-Diu0/edit?usp=sharing)
## Motivation
The inspiration for this project is the lackluster Microsoft OneDrive desktop client.  It often has issues keeping files in sync, skipping files, and broken/missing installation, which is problematic since it is built into Windows.  A wider problem with cloud storage for many users is the privacy of sensitive data that they may want backed up, but not accessible to the provider or to any security breaches of the provider.  Finally, the user may want to have their files distributed in the local file system differently than in the remote one.  Namely, they may want large media assets, such as image and video files from cameras, to be stored on a larger external drive as small SSD’s become more and more popular as primary drives.
## Introduction
The goal of this project is to produce a cross-platform cloud storage client backend that manages local storage of remote files.  This backend will be provider-agnostic, meaning any remote service, provided they offer a certain set of features, can be used.  Multiple accounts for each of these services will also be supported. 
 
There will be an optional encryption feature for remote files as well as a way to migrate an existing, unencrypted account.  In addition to encrypting file contents, the names can be encrypted and the directory structure flattened to further protect information.

Finally, the local representation of the remote files need not strictly follow the remote structure.  Locally, there may be multiple different locations that store data from a particular folder in the remote separately from the primary/default location.
