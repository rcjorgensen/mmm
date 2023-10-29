#!/bin/sh

set -xe

outfile=REPORT.md

echo "# Recipizer Report" > $outfile
echo "" >> $outfile
echo "## Recipes ordered by fewest missing ingredients" >> $outfile
echo "" >> $outfile
echo "Helpful for finding recipes that require fewest new ingredients to be bought" >> $outfile
echo "" >> $outfile
r7r show-recipes --order-by-missing-ingredients --markdown >> $outfile
