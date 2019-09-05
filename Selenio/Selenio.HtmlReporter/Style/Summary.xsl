<?xml version="1.0" encoding="ISO-8859-1"?>
<!-- Edited by XMLSpy� -->
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <html>
      <link type="text/css" rel="stylesheet" href="Template\Style.css"/>
      <body>
        <div class="heading-main">
          <div>
            <b>Test Summary</b>
          </div>
          <br />
          <div id="test_case_summary">
            <div class="testrundetails">
              <b>Test name: </b>
              <label>
                <xsl:value-of select="tests/@name"/>
              </label>
            </div>
            <div class="testrundetails">
              <b>OS version: </b>
              <label>
                <xsl:value-of select="tests/@osversion"/>
              </label>
            </div>
            <div class="testrundetails">
              <b>Browser name: </b>
              <label>
                <xsl:value-of select="tests/@browsername"/>
              </label>
            </div>
            <div class="testrundetails">
              <b>Run type: </b>
              <label>
                <xsl:value-of select="tests/@runtype"/>
              </label>
            </div>
          </div>
        </div>

        <div>
          <table class="summary">
            <tr class="heading">
              <th class="heading">Test No.</th>
              <th class="heading">Test Name</th>
              <th class="heading">Description (from Script)</th>
              <th class="heading">Result</th>
              <th class="heading">Start Time</th>
              <th class="heading">End Time</th>
              <th class="heading">Run Time (Seconds)</th>
            </tr>
            <xsl:for-each select="tests/codedui">
              <tr>
                <xsl:variable name="TestLink">
                  <xsl:value-of select="file" />
                </xsl:variable>
                <xsl:choose>
                  <xsl:when test="status = 'InProgress'">
                    <td class="valuenotrun">
                      <xsl:value-of select="testid"/>
                    </td>
                    <td class="valuenotrun">
                      <a href="{$TestLink}">
                        <xsl:value-of select="@name"/>
                      </a>
                    </td>
                    <td class="valuenotrun">
                      <xsl:value-of select="description"/>
                    </td>
                    <td class="valuenotrun">
                      <xsl:value-of select="result"/>
                    </td>
                    <td class="valuenotrun">
                      <xsl:value-of select="starttime"/>
                    </td>
                    <td class="valuenotrun">
                      <xsl:value-of select="endtime"/>
                    </td>
                    <td class="valuenotrun">
                      <xsl:value-of select="runtime"/>
                    </td>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:if test="result = 'Passed'">
                      <td class="valuepass">
                        <xsl:value-of select="testid"/>
                      </td>
                      <td class="valuepass">
                        <a href="{$TestLink}">
                          <xsl:value-of select="@name"/>
                        </a>
                      </td>
                      <td class="valuepass">
                        <xsl:value-of select="description"/>
                      </td>
                      <td class="valuepass">
                        <xsl:value-of select="result"/>
                      </td>
                      <td class="valuepass">
                        <xsl:value-of select="starttime"/>
                      </td>
                      <td class="valuepass">
                        <xsl:value-of select="endtime"/>
                      </td>
                      <td class="valuepass">
                        <xsl:value-of select="runtime"/>
                      </td>
                    </xsl:if>
                    <xsl:if test="result != 'Passed'">
                      <td class="valuefail">
                        <xsl:value-of select="testid"/>
                      </td>
                      <td class="valuefail">
                        <a href="{$TestLink}">
                          <xsl:value-of select="@name"/>
                        </a>
                      </td>
                      <td class="valuefail">
                        <xsl:value-of select="description"/>
                      </td>
                      <td class="valuefail">
                        <xsl:value-of select="result"/>
                      </td>
                      <td class="valuefail">
                        <xsl:value-of select="starttime"/>
                      </td>
                      <td class="valuefail">
                        <xsl:value-of select="endtime"/>
                      </td>
                      <td class="valuefail">
                        <xsl:value-of select="runtime"/>
                      </td>
                    </xsl:if>
                  </xsl:otherwise>
                </xsl:choose>
              </tr>
            </xsl:for-each>
          </table>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>