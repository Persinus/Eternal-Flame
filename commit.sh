#!/bin/bash

# Add file mới hoặc đã sửa
git status --porcelain | grep -E "^\s*M|^\?\?" | cut -c4- | while read file; do
  git add "$file"
done

# Xoá file đã bị xóa
git status --porcelain | grep "^ D" | cut -c4- | while read file; do
  git rm "$file"
done