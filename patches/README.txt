Okay y'all. Here's my work. If I'm being entirely honest I'm not 100% sure why this works like it does... Specifically there's a .0175 constant which they used, so I used it again and it worked.

If you installed git bash you should have access to the patch command.
You'll have to run the following commands from the git bash console, since they're bash script commands.
Hopefully when you extracted the file it gave you unix-style line endings - half of mine got changed to windows, so not sure how that happened.
Lastly, you'll have to be in the DTEDCapstone directory - the one containing the Translator folder.

If all else fails, the patch files are pretty human-readable.

anyway, the command is:

for i in path/to/patches/*.patch; do patch -p1 < $i; done;

Which applies each patch in this folder, after stripping off the first section of the filename referred to in the file header.

Good luck!
