package compiler

import (
	"strings"
)

func Compile(input string) string {
	result := strings.Builder{}

	result.WriteString("<!DOCTYPE html>")
	result.WriteString("<html lang=\"fa\"><head>")
	result.WriteString("<meta charset=\"utf-8\">")
	result.WriteString("</head><body>")

	lines := strings.Split(input, "\n")

	for _, line := range lines {
		trimmed := strings.TrimSpace(line)

		if strings.HasSuffix(trimmed, ":") {
			println("Tag Finded")
		}
	}

	result.WriteString("</body></html>")
	return result.String()
}
