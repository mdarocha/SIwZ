#! /usr/bin/env sh
[ "$(find server/ -name \*.cs -exec grep foreach {} \; | wc -l)" -ne 0 ] && echo "Please use LINQ instead of foreach" && exit 1
exit 0
