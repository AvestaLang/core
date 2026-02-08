package compiler

import "strings"

func Compile(input string) string {
	result := strings.Builder{}

	result.WriteString("<!DOCTYPE html>")
	result.WriteString("<html lang=\"fa\"><head>")
	result.WriteString("<meta charset=\"utf-8\">")
	result.WriteString("</head><body>")

	result.WriteString("</body></html>")
	return result.String()
}
